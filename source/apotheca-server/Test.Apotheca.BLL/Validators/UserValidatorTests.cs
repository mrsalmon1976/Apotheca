using Apotheca.BLL;
using Apotheca.BLL.Models;
using Apotheca.BLL.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Apotheca.BLL.Validators
{
    [TestFixture]
    public class UserValidatorTests
    {
        private IUserValidator _userValidator;
        private IEmailValidator _emailValidator;

        [SetUp]
        public void UserValidatorTests_SetUp()
        {
            _emailValidator = Substitute.For<IEmailValidator>();
            _userValidator = new UserValidator(_emailValidator);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("\t\r\n")]
        public void Validate_NoEmail_FailsValidation(string email)
        {
            // setup
            User user = CreateValidUser();
            user.Email = email;

            // execute
            try
            {
                _userValidator.Validate(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.IsNotNull(vex.Errors.SingleOrDefault(x => x == "Email address is required"));
            }

            _emailValidator.DidNotReceive().IsValidEmail(Arg.Any<string>());

        }

        [Test]
        public void Validate_InvalidEmail_FailsValidation()
        {
            // setup
            User user = CreateValidUser();
            user.Email = "NOMNOMNOM";
            _emailValidator.IsValidEmail(user.Email).Returns(false);

            // execute
            try
            {
                _userValidator.Validate(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.IsNotNull(vex.Errors.SingleOrDefault(x => x == "'NOMNOMNOM' is not a valid email address"));
            }

            // assert
            _emailValidator.Received(1).IsValidEmail(user.Email);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("\t\r\n")]
        public void Validate_NoFirstName_FailsValidation(string firstName)
        {
            // setup
            User user = CreateValidUser();
            user.FirstName = firstName;
            _emailValidator.IsValidEmail(user.Email).Returns(true);

            // execute
            try
            {
                _userValidator.Validate(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.IsNotNull(vex.Errors.SingleOrDefault(x => x == "First name is required"));
            }

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("\t\r\n")]
        public void Validate_NoLastName_FailsValidation(string lastName)
        {
            // setup
            User user = CreateValidUser();
            user.LastName = lastName;
            _emailValidator.IsValidEmail(user.Email).Returns(true);

            // execute
            try
            {
                _userValidator.Validate(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.IsNotNull(vex.Errors.SingleOrDefault(x => x == "Last name is required"));
            }
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validate_NoPassword_FailsValidation(string password)
        {
            // setup
            User user = CreateValidUser();
            user.Password = password;
            _emailValidator.IsValidEmail(user.Email).Returns(true);

            // execute
            try
            {
                _userValidator.Validate(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.IsNotNull(vex.Errors.SingleOrDefault(x => x == "Password is required"));
            }
        }

        [TestCase("     ")]
        [TestCase("\t\r\n")]
        [TestCase("pass")]
        [TestCase("passwor")]
        public void Validate_InvalidPassword_FailsValidation(string password)
        {
            // setup
            User user = CreateValidUser();
            user.Password = password;
            _emailValidator.IsValidEmail(user.Email).Returns(true);

            // execute
            try
            {
                _userValidator.Validate(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.IsNotNull(vex.Errors.SingleOrDefault(x => x == "Password must be at least 8 characters"));
            }
        }

        [TestCase]
        public void Validate_IsValidUser_PassesValidation()
        {
            // setup
            User user = CreateValidUser();                
            _emailValidator.IsValidEmail(user.Email).Returns(true);

            // execute
            _userValidator.Validate(user);

            // assert
            _emailValidator.Received(1).IsValidEmail(user.Email);
        }

        private User CreateValidUser()
        {
            User user = new User()
            {
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Smith",
                Password = "123456789"
            };
            return user;
        }


    }
}
