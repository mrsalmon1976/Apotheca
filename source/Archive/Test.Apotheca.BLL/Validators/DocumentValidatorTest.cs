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

namespace Test.Apotheca.BLL.Validators
{
    [TestFixture]
    public class DocumentValidatorTest
    {
        private IDocumentValidator _documentValidator;

        [SetUp]
        public void DocumentValidatorTest_SetUp()
        {
            _documentValidator = new DocumentValidator();
        }

        [Test]
        public void Validate_DocumentNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _documentValidator.Validate(null));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_InvalidVersionNumber_FailsValidation(int versionNo)
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            doc.VersionNo = versionNo;
            AssertValidationFailed(doc, "Version number");
        }

        [Test]
        public void Validate_InvalidName_FailsValidation()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            doc.FileName = "";
            AssertValidationFailed(doc, "File name");
        }

        [Test]
        public void Validate_InvalidExtension_FailsValidation()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            doc.Extension = "";
            AssertValidationFailed(doc, "Extension");
        }

        [Test]
        public void Validate_InvalidFileContents_FailsValidation()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            doc.FileContents = new byte[] { };
            AssertValidationFailed(doc, "File contents");
        }

        [Test]
        public void Validate_InvalidCreatedByUserId_FailsValidation()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            doc.CreatedByUserId = Guid.Empty;
            AssertValidationFailed(doc, "User");
        }


        [Test]
        public void Validate_IsValidObject_PassesValidation()
        {
            DocumentEntity doc = TestEntityHelper.CreateDocumentWithData();
            _documentValidator.Validate(doc);
        }

        /// <summary>
        /// Helper method to check validation failed and the errors collections contains an expected message.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="validationMsg"></param>
        private void AssertValidationFailed(DocumentEntity document, string validationMsg)
        {
            try
            {
                _documentValidator.Validate(document);
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
