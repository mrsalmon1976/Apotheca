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
    public class UserValidatorTest
    {
        private IStringValidator _stringValidator;
        private IUserRepository _userRepository;
        private IUserValidator _userValidator;

        [SetUp]
        public void UserValidatorTest_SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _stringValidator = Substitute.For<IStringValidator>();
            _userValidator = new UserValidator(_userRepository, _stringValidator);
        }

        [Test]
        public void Validate_UserNull_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _userValidator.Validate(null));
        }

        [Test]
        public void Validate_InvalidEmailAddress_FailsValidation()
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            user.Email = "";
            _stringValidator.IsValidEmailAddress(user.Email).Returns(false);
            AssertValidationFailed(user, "Email address");
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidFirstName_FailsValidation(string firstName)
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            user.FirstName = firstName;
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            AssertValidationFailed(user, "First name");
        }


        [TestCase(null)]
        [TestCase("")]
        public void Validate_InvalidPassword_FailsValidation(string password)
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            user.Password = password;
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            AssertValidationFailed(user, "Password");
        }

        [TestCase("bollocks")]
        [TestCase("")]
        [TestCase(null)]
        public void Validate_InvalidRole_FailsValidation(string role)
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            user.Role = role;
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            AssertValidationFailed(user, "Role");
        }

        [Test]
        public void Validate_NewUserAlreadyExists_FailsValidation()
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            UserEntity existingUser = TestEntityHelper.CreateUserWithData();
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            _userRepository.GetUserByEmail(user.Email).Returns(existingUser);
            AssertValidationFailed(user, "user already exists");
        }

        [Test]
        public void Validate_IsValidObject_PassesValidation()
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            _userValidator.Validate(user);
        }

        [Test]
        public void Validate_OnValidation_CheckDependencies()
        {
            UserEntity user = TestEntityHelper.CreateUserWithData();
            _stringValidator.IsValidEmailAddress(user.Email).Returns(true);
            _userValidator.Validate(user);

            _stringValidator.Received(1).IsValidEmailAddress(user.Email);
            _userRepository.Received(1).GetUserByEmail(user.Email);
        }

        /// <summary>
        /// Helper method to check validation failed and the errors collections contains an expected message.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="validationMsg"></param>
        private void AssertValidationFailed(UserEntity user, string validationMsg)
        {
            try
            {
                _userValidator.Validate(user);
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
