using Apotheca.BLL.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Apotheca.BLL.Models;
using Test.Apotheca.TestHelpers;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Repositories;
using Apotheca.Validators;
using Apotheca.ViewModels.User;
using Apotheca.ViewModels.Document;

namespace Test.Apotheca.Validators
{
    [TestFixture]
    public class DocumentViewModelValidatorTest
    {
        private IDocumentViewModelValidator _documentValidator;

        [SetUp]
        public void DocumentViewModelValidatorTest_SetUp()
        {
            _documentValidator = new DocumentViewModelValidator();
        }

        [Test]
        public void Validate_ModelNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _documentValidator.Validate(null));
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidFileName_FailsValidation(string fileName)
        {
            DocumentViewModel model = TestViewModelHelper.CreateDocumentViewModelWithData();
            model.FileName = fileName;
            AssertValidationFailed(model, "File name");
        }


        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidUploadedFileName_FailsValidation(string fileName)
        {
            DocumentViewModel model = TestViewModelHelper.CreateDocumentViewModelWithData();
            model.UploadedFileName = fileName;
            AssertValidationFailed(model, "Uploaded file name");
        }

        [Test]
        public void Validate_IsValidObject_PassesValidation()
        {
            DocumentViewModel doc = TestViewModelHelper.CreateDocumentViewModelWithData();
            List<string> errors = _documentValidator.Validate(doc);
            Assert.AreEqual(0, errors.Count);
        }

        /// <summary>
        /// Helper method to check validation failed and the errors collections contains an expected message.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="validationMsg"></param>
        private void AssertValidationFailed(DocumentViewModel model, string validationMsg)
        {
            List<string> errors = _documentValidator.Validate(model);
            string error = errors.FirstOrDefault(x => x.Contains(validationMsg));
            Assert.IsNotNull(error, "Unable to find expected validation error in Errors collection");
        }


    }
}
