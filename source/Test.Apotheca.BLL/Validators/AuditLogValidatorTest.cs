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
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Data;

namespace Test.Apotheca.BLL.Validators
{
    [TestFixture]
    public class AuditLogValidatorTest
    {
        private IAuditLogValidator _auditLogValidator;
        private IUnitOfWork _unitOfWork;
        private IAuditLogRepository _auditLogRepo;
 
        [SetUp]
        public void AuditLogValidatorTest_SetUp()
        {
            _auditLogRepo = Substitute.For<IAuditLogRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.AuditLogRepo.Returns(_auditLogRepo);

            _auditLogValidator = new AuditLogValidator(_unitOfWork);
        }

        [Test]
        public void Validate_AuditLogNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _auditLogValidator.Validate(null));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("      ")]
        public void Validate_InvalidAction_FailsValidation(string action)
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            auditLog.Action = action;
            AssertValidationFailed(auditLog, "Action");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("      ")]
        public void Validate_InvalidEntity_FailsValidation(string entity)
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            auditLog.Entity = entity;
            AssertValidationFailed(auditLog, "Entity");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("      ")]
        public void Validate_InvalidKey_FailsValidation(string key)
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            auditLog.Key = key;
            AssertValidationFailed(auditLog, "Key");
        }

        [Test]
        public void Validate_InvalidUserId_FailsValidation()
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            auditLog.UserId = Guid.Empty;
            AssertValidationFailed(auditLog, "User Id");
        }

        [Test]
        public void Validate_IsValidObject_PassesValidation()
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();

            _auditLogValidator.Validate(auditLog);
        }

        /// <summary>
        /// Helper method to check validation failed and the errors collections contains an expected message.
        /// </summary>
        /// <param name="auditLog"></param>
        /// <param name="validationMsg"></param>
        private void AssertValidationFailed(AuditLogEntity auditLog, string validationMsg)
        {
            try
            {
                _auditLogValidator.Validate(auditLog);
                Assert.Fail("Validation did not fail as expected");
            }
            catch (ValidationException ex)
            {
                string error = ex.Errors.FirstOrDefault(x => x.Contains(validationMsg));
                Assert.IsNotNull(error, "Unable to find expected validation error in Errors collection");
            }
        }


    }
}
