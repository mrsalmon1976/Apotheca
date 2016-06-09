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
    public class SaveDocumentCommandTest
    {
        private ISaveDocumentCommand _command;

        private IUnitOfWork _unitOfWork;
        private IDbTransaction _dbTransaction;

        private IDocumentValidator _documentValidator;
        private IDocumentRepository _documentRepo;
        private IDocumentVersionRepository _documentVersionRepo;

        [SetUp]
        public void CreateDocumentCommandTest_SetUp()
        {
            _dbTransaction = Substitute.For<IDbTransaction>();

            _documentValidator = Substitute.For<IDocumentValidator>();
            _documentRepo = Substitute.For<IDocumentRepository>();
            _documentVersionRepo = Substitute.For<IDocumentVersionRepository>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.DocumentRepo.Returns(_documentRepo);
            _unitOfWork.DocumentVersionRepo.Returns(_documentVersionRepo);
            
            _command = new SaveDocumentCommand(_unitOfWork, _documentValidator);
        }

        [Test]
        public void Execute_NoDocument_ThrowsException()
        {
            _command.Document = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_NoTransaction_ThrowsException()
        {
            IDbTransaction transaction = null;
            _unitOfWork.CurrentTransaction.Returns(transaction);
            _command.Document = TestEntityHelper.CreateDocument();
            Assert.Throws(typeof(InvalidOperationException), () => _command.Execute());
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
        public void Execute_NewDocument_SetsVersionToOne()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocument();
            doc.VersionNo = 0;
            _documentRepo.When(x => x.Create(doc)).Do((c) => { doc.Id = Guid.NewGuid(); });

            _command.Document = doc;
            _command.Execute();

            Assert.AreEqual(1, doc.VersionNo);
            _documentVersionRepo.DidNotReceive().GetVersionCount(Arg.Any<Guid>());
        }

        [Test]
        public void Execute_ExistingDocument_IncrementsVersion()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            doc.VersionNo = 0;
            _documentVersionRepo.GetVersionCount(doc.Id.Value).Returns(5);

            _command.Document = doc;
            _command.Execute();

            Assert.AreEqual(6, doc.VersionNo);
            _documentVersionRepo.Received(1).GetVersionCount(doc.Id.Value);
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
        public void Execute_NewDocument_CreatesAndReturnsId()
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
        public void Execute_ExistingDocument_Updates()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();

            _command.Document = doc;
            Guid result = _command.Execute();

            _documentRepo.Received(1).Update(doc);
            Assert.AreEqual(doc.Id, result);
        }

        [Test]
        public void Execute_WithDocument_SavesVersion()
        {
            Guid id = Guid.NewGuid();
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();

            _command.Document = doc;
            Guid result = _command.Execute();

            _documentVersionRepo.Received(1).Create(doc);
        }

    }
}
