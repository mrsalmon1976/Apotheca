using Apotheca.BLL.Models;
using Apotheca.BLL.Services;
using Apotheca.Web.API;
using Apotheca.Web.API.Controllers;
using Apotheca.Web.API.Services;
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
        private IAccountViewModelService _accountViewModelService;

        [SetUp]
        public void Setup()
        {
            _authService = Substitute.For<IAuthService>();
            _userService = Substitute.For<IUserService>();
            _accountViewModelService = Substitute.For<IAccountViewModelService>();
            _accountController = new AccountController(_authService, _userService, _accountViewModelService);
        }

        [Test]
        public async Task Login_ModelValidationFails_ReturnsValidationProblem()
        {
            _accountController.ModelState.AddModelError("Email", "error");

            var result = await _accountController.Login(new LoginViewModel()) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            await _authService.Received(0).Authenticate(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task Login_AuthenticationFails_ReturnsValidationProblem()
        {
            LoginViewModel userViewModel = CreateUserLoginViewModel();
            User user = null;
            _authService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).Returns(user);

            var result = await _accountController.Login(userViewModel) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            await _authService.Received(1).Authenticate(userViewModel.Email, userViewModel.Password);

            string error = result.Value as string;
            Assert.AreEqual(error, "No user found matching the supplied email address/password");

        }

        [Test]
        public async Task Login_AuthenticationSucceedsButRegistrationNotCompleted_ReturnsUnauthorized()
        {
            LoginViewModel userViewModel = CreateUserLoginViewModel();
            User user = new User()
            {
                Email = userViewModel.Email,
                Password = userViewModel.Password,
                Token = Guid.NewGuid().ToString(),
                RegistrationCompleted = null
            };
            _authService.Authenticate(userViewModel.Email, userViewModel.Password).Returns(user);

            var result = await _accountController.Login(userViewModel) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            // we should have received an auth call
            await _authService.Received(1).Authenticate(user.Email, user.Password);

            string error = result.Value as string;
            Assert.AreEqual(error, "Registration has not been completed for this account");

        }

        [Test]
        public async Task Login_AuthenticationSucceedsAndRegistered_ReturnsOk()
        {
            AppMap.Reset();
            AppMap.Configure();

            LoginViewModel loginViewModel = CreateUserLoginViewModel();
            User user = new User()
            {
                Email = loginViewModel.Email,
                Password = loginViewModel.Password,
                Token = Guid.NewGuid().ToString(),
                RegistrationCompleted = DateTime.Now
            };
            _authService.Authenticate(loginViewModel.Email, loginViewModel.Password).Returns(user);

            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id
            };
            _accountViewModelService.LoadUserWithStores(user).Returns(userViewModel);

            // execute 
            var result = await _accountController.Login(loginViewModel) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // we should have received an auth call
            await _authService.Received(1).Authenticate(user.Email, user.Password);

            // the result should have been a user view model with the token from above, the email from above, and the password removed
            UserViewModel returnValue = result.Value as UserViewModel;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(user.Id, returnValue.Id);
            await _accountViewModelService.Received(1).LoadUserWithStores(user);
        }

        private static LoginViewModel CreateUserLoginViewModel(string email = "test@test.com", string password = "testpassword")
        {
            LoginViewModel userViewModel = new LoginViewModel()
            {
                Email = email,
                Password = password,
            };
            return userViewModel;
        }

    }
}