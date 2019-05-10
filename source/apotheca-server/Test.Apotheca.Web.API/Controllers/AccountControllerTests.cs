using Apotheca.BLL.Models;
using Apotheca.BLL.Services;
using Apotheca.Web.API;
using Apotheca.Web.API.Controllers;
using Apotheca.Web.API.ViewModels;
using Apotheca.Web.API.ViewModels.Account;
using Apotheca.Web.API.ViewModels.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test.Apotheca.Web.API.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private IAuthService _authService;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _authService = Substitute.For<IAuthService>();
            _userService = Substitute.For<IUserService>();
            _accountController = new AccountController(_authService, _userService);
        }

        [Test]
        public void Login_ModelValidationFails_ReturnsValidationProblem()
        {
            _accountController.ModelState.AddModelError("Email", "error");

            var result = _accountController.Login(new LoginViewModel()) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            _authService.Received(0).Authenticate(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void Login_AuthenticationFails_ReturnsValidationProblem()
        {
            LoginViewModel userViewModel = CreateUserLoginViewModel();
            Task<User> userTask = Task.FromResult<User>(null);
            _authService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).Returns(userTask);

            var result = _accountController.Login(userViewModel) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            _authService.Received(1).Authenticate(userViewModel.Email, userViewModel.Password);

            string error = result.Value as string;
            Assert.AreEqual(error, "No user found matching the supplied email address/password");

        }

        [Test]
        public void Login_AuthenticationSucceedsButRegistrationNotCompleted_ReturnsUnauthorized()
        {
            LoginViewModel userViewModel = CreateUserLoginViewModel();
            User user = new User();
            user.Email = userViewModel.Email;
            user.Password = userViewModel.Password;
            user.Token = Guid.NewGuid().ToString();
            user.RegistrationCompleted = null;
            Task<User> userTask = Task.FromResult<User>(user);
            _authService.Authenticate(userViewModel.Email, userViewModel.Password).Returns(userTask);

            var result = _accountController.Login(userViewModel) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            // we should have received an auth call
            _authService.Received(1).Authenticate(user.Email, user.Password);

            string error = result.Value as string;
            Assert.AreEqual(error, "Registration has not been completed for this account");

        }

        [Test]
        public void Login_AuthenticationSucceedsAndRegistered_ReturnsOk()
        {
            AppMap.Reset();
            AppMap.Configure();

            LoginViewModel userViewModel = CreateUserLoginViewModel();
            User user = new User();
            user.Email = userViewModel.Email;
            user.Password = userViewModel.Password;
            user.Token = Guid.NewGuid().ToString();
            user.RegistrationCompleted = DateTime.Now;
            Task<User> userTask = Task.FromResult<User>(user);
            _authService.Authenticate(userViewModel.Email, userViewModel.Password).Returns(userTask);

            var result = _accountController.Login(userViewModel) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // we should have received an auth call
            _authService.Received(1).Authenticate(user.Email, user.Password);

            // the result should have been a user view model with the token from above, the email from above, and the password removed
            UserViewModel returnValue = result.Value as UserViewModel;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(user.Email, returnValue.Email);
            Assert.AreEqual(user.Token, returnValue.Token);
        }

        private static LoginViewModel CreateUserLoginViewModel(string email = "test@test.com", string password = "testpassword")
        {
            LoginViewModel userViewModel = new LoginViewModel();
            userViewModel.Email = email;
            userViewModel.Password = password;
            return userViewModel;
        }

    }
}