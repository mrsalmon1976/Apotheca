using Apotheca.BLL.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Apotheca.BLL.Validators
{
    [TestFixture]
    public class EmailValidatorTests
    {
        private IEmailValidator _emailValidator;

        [SetUp]
        public void EmailValidatorTests_SetUp()
        {
            _emailValidator = new EmailValidator();
        }

        [TestCase("a@a.co")]
        [TestCase("aa@aa.com")]
        public void IsValidEmail_ValidEmail_ReturnsTrue(string email)
        {
            bool result = _emailValidator.IsValidEmail(email);
            Assert.IsTrue(result, $"Expected true, but got false with email {email}");
        }

        [TestCase("aa")]
        [TestCase("aa.com")]
        [TestCase("@aa.com")]
        [TestCase("aa@aa")]
        [TestCase("a@a.c")]
        public void IsValidEmail_InvalidEmail_ReturnsFalse(string email)
        {
            bool result = _emailValidator.IsValidEmail(email);
            Assert.IsFalse(result, $"Expected false, but got true with email {email}");
        }

    }
}
