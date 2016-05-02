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

namespace Test.Apotheca.Validators
{
    [TestFixture]
    public class UserViewModelValidatorTest
    {
        private IStringValidator _stringValidator;
        private IUserViewModelValidator _userValidator;

        [SetUp]
        public void UserViewModelValidatorTest_SetUp()
        {
            _stringValidator = Substitute.For<IStringValidator>();
            _userValidator = new UserViewModelValidator(_stringValidator);
        }

        [Test]
        public void Validate_ModelNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _userValidator.Validate(null));
        }

        [Test]
        public void Validate_InvalidEmailAddress_FailsValidation()
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            model.Email = "";
            _stringValidator.IsValidEmailAddress(model.Email).Returns(false);
            AssertValidationFailed(model, "Email address");
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidFirstName_FailsValidation(string firstName)
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            model.FirstName = firstName;
            _stringValidator.IsValidEmailAddress(model.Email).Returns(true);
            AssertValidationFailed(model, "First name");
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("abc")]
        [TestCase("1234567")]
        public void Validate_InvalidPassword_FailsValidation(string password)
        {
            UserViewModel user = TestViewModelHelper.CreateUserViewModelWithData();
            user.Password = password;
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            AssertValidationFailed(user, "Password must");
        }

        public void Validate_PasswordConfirmFails_FailsValidation(string password)
        {
            UserViewModel user = TestViewModelHelper.CreateUserViewModelWithData();
            user.Password = password;
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            AssertValidationFailed(user, "Password and confirmation password");
        }

        [TestCase("bollocks")]
        [TestCase("")]
        [TestCase(null)]
        public void Validate_InvalidRole_FailsValidation(string role)
        {
            UserViewModel user = TestViewModelHelper.CreateUserViewModelWithData();
            user.Role = role;
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            AssertValidationFailed(user, "Role");
        }

        [Test]
        public void Validate_IsValidObject_PassesValidation()
        {
            UserViewModel user = TestViewModelHelper.CreateUserViewModelWithData();
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            List<string> errors = _userValidator.Validate(user);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void Validate_OnValidation_CheckDependencies()
        {
            UserViewModel user = TestViewModelHelper.CreateUserViewModelWithData();
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            _userValidator.Validate(user);

            _stringValidator.Received(1).IsValidEmailAddress(user.Email);
        }

        /// <summary>
        /// Helper method to check validation failed and the errors collections contains an expected message.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="validationMsg"></param>
        private void AssertValidationFailed(UserViewModel model, string validationMsg)
        {
            List<string> errors = _userValidator.Validate(model);
            string error = errors.FirstOrDefault(x => x.Contains(validationMsg));
            Assert.IsNotNull(error, "Unable to find expected validation error in Errors collection");
        }


    }
}
