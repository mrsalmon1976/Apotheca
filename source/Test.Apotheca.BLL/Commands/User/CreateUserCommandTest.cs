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

namespace Test.Apotheca.BLL.Commands.User
{
    [TestFixture]
    public class CreateUserCommandTest
    {
        private ICreateUserCommand _command;
        private IUserValidator _userValidator;

        [SetUp]
        public void CreateUserCommandTest_SetUp()
        {
            _userValidator = Substitute.For<IUserValidator>();
            _command = new CreateUserCommand(_userValidator);
        }

        [Test]
        public void Execute_UserNull_ThrowsException()
        {
            _command.User = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_WithUser_Validate()
        {
            _command.User = TestEntityHelper.CreateUser();
            _command.Execute();
            _userValidator.Received(1).Validate(_command.User);
        }

    }
}
