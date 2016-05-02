using Apotheca.BLL.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.BLL.Validators
{
    [TestFixture]
    public class StringValidatorTest
    {
        private IStringValidator _stringValidator;

        [SetUp]
        public void StringValidatorTest_SetUp()
        {
            _stringValidator = new StringValidator();
        }

        [TestCase("matt@test.com", true)]
        [TestCase("a@a.a", true)]
        [TestCase("a@a", false)]
        [TestCase(null, false)]
        [TestCase("", false)]
        public void IsValidEmailAddress_CheckValidation(string email, bool expectedResult)
        {
            bool result = _stringValidator.IsValidEmailAddress(email);
            Assert.AreEqual(expectedResult, result, "Validation on address " + email + " did not return the expected result");
        }
    }
}
