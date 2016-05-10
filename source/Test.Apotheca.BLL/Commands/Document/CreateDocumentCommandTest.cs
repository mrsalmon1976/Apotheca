using Apotheca.BLL.Commands.User;
using Apotheca.BLL.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Apotheca.BLL.Models;
using Test.Apotheca.BLL.TestHelpers;
using Apotheca.BLL.Security;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Data;
using System.Data;
using Apotheca.BLL.Commands.Document;

namespace Test.Apotheca.BLL.Commands.Document
{
    [TestFixture]
    public class CreateDocumentCommandTest
    {
        private ICreateDocumentCommand _command;

        private IDbContext _dbContext;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        private IDocumentValidator _documentValidator;
        private IDocumentRepository _documentRepo;

        [SetUp]
        public void CreateDocumentCommandTest_SetUp()
        {
            _dbContext = Substitute.For<IDbContext>();
            _dbConnection = Substitute.For<IDbConnection>();
            _dbContext.GetConnection().Returns(_dbConnection);
            _dbTransaction = Substitute.For<IDbTransaction>();
            _dbContext.BeginTransaction().Returns(_dbTransaction);

            _documentValidator = Substitute.For<IDocumentValidator>();
            _documentRepo = Substitute.For<IDocumentRepository>();
            
            _command = new CreateDocumentCommand(_dbContext, _documentValidator, _documentRepo);
        }

        [Test]
        public void Execute_UserDocument_ThrowsException()
        {
            _command.Document = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_WithDocument_Validate()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocument();
            _documentRepo.When(x => x.Create(doc)).Do((c) => { doc.Id = Guid.NewGuid(); });

            _command.Document = doc;
            _command.Execute();

            _documentValidator.Received(1).Validate(_command.Document);
        }

        [Test]
        public void Execute_WithDocument_SetsCreatedOn()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocument();
            doc.CreatedOn = null;
            _documentRepo.When(x => x.Create(doc)).Do((c) => { doc.Id = Guid.NewGuid(); });

            DateTime dtBefore = DateTime.UtcNow;
            _command.Document = doc;
            _command.Execute();
            DateTime dtAfter = DateTime.UtcNow;

            Assert.IsNotNull(doc.CreatedOn);
            Assert.LessOrEqual(dtBefore, doc.CreatedOn);
            Assert.GreaterOrEqual(dtAfter, doc.CreatedOn);
        }

        [Test]
        public void Execute_WithDocument_SavesAndReturnsId()
        {
            Guid id = Guid.NewGuid();
            DocumentEntity doc = TestEntityHelper.CreateDocument();
            _documentRepo.When(x => x.Create(doc)).Do((c) => { doc.Id = id; });

            _command.Document = doc;
            Guid result = _command.Execute();

            _documentRepo.Received(1).Create(doc);
            Assert.AreEqual(id, result);
        }

        [Test]
        public void Execute_WithDocument_UsesTransaction()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocument();
            _documentRepo.When(x => x.Create(doc)).Do((c) => { doc.Id = Guid.NewGuid(); });

            _command.Document = doc;
            Guid result = _command.Execute();

            _dbContext.Received(1).BeginTransaction();
            _dbTransaction.Received(1).Commit();
        }

    }
}
