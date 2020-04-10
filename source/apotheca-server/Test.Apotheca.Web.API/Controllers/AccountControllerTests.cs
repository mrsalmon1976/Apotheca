using Apotheca.Auth;
using Apotheca.Auth.Models;
using Apotheca.Web.API;
using Apotheca.Web.API.Controllers;
using Apotheca.Web.API.ViewModels.Account;
using Apotheca.Web.API.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Test.Apotheca.Web.API.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private IAmazonCognitoProvider _cognitoProvider;

        [SetUp]
        public void Setup()
        {
            _cognitoProvider = Substitute.For<IAmazonCognitoProvider>();
            _accountController = new AccountController(_cognitoProvider);
        }

        #region Login Tests

        [Test]
        public async Task Login_ModelValidationFails_ReturnsValidationProblem()
        {
            _accountController.ModelState.AddModelError("Email", "error");

            var result = await _accountController.Login(new LoginViewModel()) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            await _cognitoProvider.Received(0).LoginAsync(Arg.Any<string>(), Arg.Any<string>());
            await _cognitoProvider.Received(0).GetUser(Arg.Any<string>());
        }

        [Test]
        public async Task Login_AuthenticationSucceedsAndRegistered_ReturnsOk()
        {
            AppMap.Reset();
            AppMap.Configure();

            LoginViewModel loginViewModel = CreateUserLoginViewModel();

            // set up the login result
            LoginResult loginResult = new LoginResult()
            {
                AccessToken = Guid.NewGuid().ToString(),
                IdToken = Guid.NewGuid().ToString(),
                ExpiresIn = new Random().Next(100, 1000)
            };
            _cognitoProvider.LoginAsync(loginViewModel.Email, loginViewModel.Password).Returns(loginResult);

            // set up the call to get the user
            UserResult userResult = new UserResult
            {
                Email = loginViewModel.Email,
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
            _cognitoProvider.GetUser(loginResult.AccessToken).Returns(userResult);

            // execute 
            var result = await _accountController.Login(loginViewModel) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // we should have received an auth call
            await _cognitoProvider.Received(1).LoginAsync(loginViewModel.Email, loginViewModel.Password);
            await _cognitoProvider.Received(1).GetUser(loginResult.AccessToken);

            // the result should have been a user view model with the token from above, the email from above, and the password removed
            UserViewModel returnValue = result.Value as UserViewModel;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(userResult.Email, returnValue.Email);
            Assert.AreEqual(userResult.FirstName, returnValue.FirstName);
            Assert.AreEqual(userResult.LastName, returnValue.LastName);
            Assert.AreEqual(loginResult.IdToken, returnValue.Token);
        }

        #endregion

        #region Register Tests

        [Test]
        public void Register_ModelValidationFails_ReturnsValidationProblem()
        {
            AppMap.Reset();
            AppMap.Configure();

            _accountController.ModelState.AddModelError("Email", "error");
            RegisterViewModel registerViewModel = new RegisterViewModel();

            BadRequestObjectResult actionResult = _accountController.Register(registerViewModel).Result as BadRequestObjectResult;
            Assert.IsNotNull(actionResult);

            ValidationProblemDetails problemDetails = actionResult.Value as ValidationProblemDetails;
            Assert.IsNotNull(problemDetails);
            Assert.AreEqual(1, problemDetails.Errors.Count);
        }

        [Test]
        public void Register_ModelValidates_AuthenticatesWithAwsAndReturnsOkResult()
        {
            AppMap.Reset();
            AppMap.Configure();

            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.Email = "test@apotheca.com";
            registerViewModel.Password = Guid.NewGuid().ToString();
            registerViewModel.FirstName = Guid.NewGuid().ToString();
            registerViewModel.LastName = Guid.NewGuid().ToString();


            OkResult actionResult = _accountController.Register(registerViewModel).Result as OkResult;
            Assert.IsNotNull(actionResult);

            _cognitoProvider.Received(1).RegisterAsync(registerViewModel.Email, registerViewModel.Password, registerViewModel.FirstName, registerViewModel.LastName);

        }

        #endregion

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