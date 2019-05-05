﻿using Apotheca.BLL;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Security;
using Apotheca.BLL.Services;
using Apotheca.BLL.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.BLL.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;

        private IUserRepository _userRepo;
        private IUserValidator _userValidator;
        private IPasswordProvider _passwordProvider;

        [SetUp]
        public void UserServiceTests_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _userValidator = Substitute.For<IUserValidator>();
            _passwordProvider = Substitute.For<IPasswordProvider>();

            _userService = new UserService(_userRepo, _userValidator, _passwordProvider);
        }

        [Test]
        public void CreateUser_InvalidUser_ThrowsValidationException()
        {
            User user = new User();
            _userValidator.When(x => x.Validate(Arg.Any<User>())).Throw(new ValidationException(new string[] { "error" }));

            try
            {
                _userService.CreateUser(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.AreEqual("error", vex.Errors.First());
            }

            _passwordProvider.DidNotReceive().GenerateSalt();
            _userRepo.DidNotReceive().Insert(Arg.Any<User>());
        }

        [Test]
        public void CreateUser_EmailAddressExists_ThrowsValidationException()
        {
            User user = new User();
            user.Email = "test@test.com";

            Task<User> userExistsTask = Task.FromResult<User>(new User());
            _userRepo.GetUserByEmail(user.Email).Returns(userExistsTask);

            try
            {
                _userService.CreateUser(user);
            }
            catch (ValidationException vex)
            {
                Assert.AreEqual(1, vex.Errors.Count());
                Assert.AreEqual("A user with this email address already exists", vex.Errors.First());
            }

            _passwordProvider.DidNotReceive().GenerateSalt();
            _userRepo.DidNotReceive().Insert(Arg.Any<User>());
        }

        [Test]
        public void CreateUser_ValidUser_ReturnsUpdatedUser()
        {
            User user = CreateValidUser();
            byte[] salt = new byte[256];
            new Random().NextBytes(salt);
            string hashedPassword = Guid.NewGuid().ToString();

            _passwordProvider.GenerateSalt().Returns(salt);
            _passwordProvider.HashPassword(user.Password, salt).Returns(hashedPassword);

            User result = _userService.CreateUser(user);

            _passwordProvider.Received(1).GenerateSalt();
            _passwordProvider.Received(1).HashPassword(user.Password, salt);
            _userRepo.Received(1).Insert(Arg.Any<User>());

            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.FirstName, result.FirstName);
            Assert.AreEqual(user.LastName, result.LastName);
            Assert.AreEqual(salt, result.Salt);
            Assert.AreEqual(hashedPassword, result.Password);

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