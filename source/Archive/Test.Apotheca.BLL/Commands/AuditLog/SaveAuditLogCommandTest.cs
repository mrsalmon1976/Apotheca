using Apotheca.BLL.Commands.AuditLog;
using Apotheca.BLL.Commands.Category;
using Apotheca.BLL.Data;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Data;
using Test.Apotheca.BLL.TestHelpers;

namespace Test.Apotheca.BLL.Commands.AuditLog
{
    [TestFixture]
    public class SaveAuditLogCommandTest
    {
        private ISaveAuditLogCommand _command;

        private IAuditLogRepository _auditLogRepo;
        private IDbTransaction _dbTransaction;
        private IUnitOfWork _unitOfWork;

        private IAuditLogValidator _auditLogValidator;

        [SetUp]
        public void CreateDocumentCommandTest_SetUp()
        {
            _auditLogRepo = Substitute.For<IAuditLogRepository>();
            _dbTransaction = Substitute.For<IDbTransaction>();
            _auditLogValidator = Substitute.For<IAuditLogValidator>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.AuditLogRepo.Returns(_auditLogRepo);
            _unitOfWork.CurrentTransaction.Returns(_dbTransaction);

            _command = new SaveAuditLogCommand(_unitOfWork, _auditLogValidator);
        }

        [Test]
        public void Execute_NoAuditLog_ThrowsException()
        {
            _command.AuditLog = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_NoTransaction_ThrowsException()
        {
            IDbTransaction transaction = null;
            _unitOfWork.CurrentTransaction.Returns(transaction);
            _command.AuditLog = TestEntityHelper.CreateAuditLogWithData();
            Assert.Throws(typeof(InvalidOperationException), () => _command.Execute());
        }


        [Test]
        public void Execute_WithAuditLog_Validate()
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            _auditLogRepo.When(x => x.Create(auditLog)).Do((c) => { auditLog.Id = new Random().Next(); });

            _command.AuditLog = auditLog;
            _command.Execute();

            _auditLogValidator.Received(1).Validate(auditLog);
        }

        [Test]
        public void Execute_WithNewAuditLog_SetsAuditDateTime()
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            _auditLogRepo.When(x => x.Create(auditLog)).Do((c) => { auditLog.Id = new Random().Next(); });

            DateTime dtBefore = DateTime.UtcNow;
            _command.AuditLog = auditLog;
            _command.Execute();
            DateTime dtAfter = DateTime.UtcNow;

            Assert.IsNotNull(auditLog.AuditDateTime);
            Assert.LessOrEqual(dtBefore, auditLog.AuditDateTime);
            Assert.GreaterOrEqual(dtAfter, auditLog.AuditDateTime);
        }

        [Test]
        public void Execute_NewAuditLog_CreatesAndReturnsId()
        {
            int id = new Random().Next();
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            _auditLogRepo.When(x => x.Create(auditLog)).Do((c) => { auditLog.Id = id; });

            _command.AuditLog = auditLog;
            int result = _command.Execute();

            _auditLogRepo.Received(1).Create(auditLog);
            Assert.AreEqual(id, result);
        }


    }
}
