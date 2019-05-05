using Apotheca.BLL.Models;
using Apotheca.BLL.Services;
using Apotheca.Web.API;
using Apotheca.Web.API.Controllers;
using Apotheca.Web.API.ViewModels;
using Apotheca.Web.API.ViewModels.Account;
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

            var result = _accountController.Login(new UserLoginViewModel()) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            _authService.Received(0).Authenticate(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void Login_AuthenticationFails_ReturnsValidationProblem()
        {
            UserLoginViewModel userViewModel = CreateUserLoginViewModel();
            Task<User> userTask = Task.FromResult<User>(null);
            _authService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).Returns(userTask);

            var result = _accountController.Login(userViewModel) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            _authService.Received(1).Authenticate(userViewModel.Email, userViewModel.Password);
        }

        [Test]
        public void Login_AuthenticationSucceeds_ReturnsValidationProblem()
        {
            UserLoginViewModel userViewModel = CreateUserLoginViewModel();
            User user = new User();
            user.Email = userViewModel.Email;
            user.Password = userViewModel.Password;
            user.Token = Guid.NewGuid().ToString();
            Task<User> userTask = Task.FromResult<User>(user);
            _authService.Authenticate(userViewModel.Email, userViewModel.Password).Returns(userTask);

            var result = _accountController.Login(userViewModel) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // we should have received an auth call
            _authService.Received(1).Authenticate(user.Email, user.Password);

            // the result should have been a user view model with the token from above, the email from above, and the password removed
            UserLoginViewModel returnValue = result.Value as UserLoginViewModel;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(user.Email, returnValue.Email);
            Assert.AreEqual(user.Token, returnValue.Token);
            Assert.IsNull(returnValue.Password);
        }

        private static UserLoginViewModel CreateUserLoginViewModel(string email = "test@test.com", string password = "testpassword")
        {
            UserLoginViewModel userViewModel = new UserLoginViewModel();
            userViewModel.Email = email;
            userViewModel.Password = password;
            return userViewModel;
        }

    }
}