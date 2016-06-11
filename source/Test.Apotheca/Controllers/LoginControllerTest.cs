using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Apotheca.Modules;
using Nancy;
using Apotheca.ViewModels.Login;
using Apotheca.Web.Results;
using Apotheca.Navigation;
using Apotheca.Web;
using Nancy.Security;
using Apotheca.BLL.Data;
using Apotheca.BLL.Security;
using Apotheca.BLL.Models;
using Test.Apotheca.BLL.TestHelpers;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class LoginControllerTest
    {
        private ILoginController _loginController;
        private IUnitOfWork _unitOfWork;
        private IPasswordProvider _passwordProvider;
        private IUserRepository _userRepo;

        [SetUp]
        public void LoginControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _passwordProvider = Substitute.For<IPasswordProvider>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.UserRepo.Returns(_userRepo);

            _loginController = new LoginController(_unitOfWork, _passwordProvider);
        }

        #region LoginGet Tests

        [Test]
        public void LoginGet_NoUsers_RedirectsToSetup()
        {
            // setup 
            _userRepo.UsersExist().Returns(false);

            // execute
            RedirectResult result = _loginController.LoginGet(null, new LoginViewModel()) as RedirectResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Actions.Setup.Default, result.Location);
            _userRepo.Received(1).UsersExist();

        }

        [Test]
        public void LoginGet_AlreadyLoggedIn_RedirectsToDashboard()
        {
            // setup 
            IUserIdentity currentUser = Substitute.For<IUserIdentity>();
            _userRepo.UsersExist().Returns(true);

            // execute
            RedirectResult result = _loginController.LoginGet(currentUser, new LoginViewModel()) as RedirectResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Actions.Dashboard, result.Location);
            _userRepo.Received(1).UsersExist();

        }

        [Test]
        public void LoginGet_UsersExist_ReturnsView()
        {
            // setup 
            _userRepo.UsersExist().Returns(true);

            // execute
            ViewResult result = _loginController.LoginGet(null, new LoginViewModel()) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Login, result.ViewName);

            LoginViewModel model = result.Model as LoginViewModel;
            Assert.IsNotNull(model);

            _userRepo.Received(1).UsersExist();

        }

        #endregion

        #region LoginPost Tests

        [TestCase("")]
        [TestCase(null)]
        [TestCase("     ")]
        public void LoginPost_EmailEmpty_Fails(string email)
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = email;
            model.Password = "testpassword";

            LoginResult result = _loginController.LoginPost(model) as LoginResult;
            
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            _userRepo.DidNotReceive().GetUserByEmailOrDefault(Arg.Any<string>());
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("     ")]
        public void LoginPost_PasswordEmpty_Fails(string password)
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = "test@test.com";
            model.Password = password;

            LoginResult result = _loginController.LoginPost(model) as LoginResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            _userRepo.DidNotReceive().GetUserByEmailOrDefault(Arg.Any<string>());
        }

        [Test]
        public void LoginPost_UserNotFound_Fails()
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = "test@test.com";
            model.Password = "password";
            UserEntity user = null;
            _userRepo.GetUserByEmailOrDefault(model.Email).Returns(user);

            LoginResult result = _loginController.LoginPost(model) as LoginResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            _userRepo.Received(1).GetUserByEmailOrDefault(model.Email);
            _passwordProvider.DidNotReceive().CheckPassword(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void LoginPost_PasswordIncorrect_Fails()
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = "test@test.com";
            model.Password = "password";
            UserEntity user = TestEntityHelper.CreateUser(email: model.Email);
            user.Password = "dsdsdsdsdsdfdf";

            _userRepo.GetUserByEmailOrDefault(model.Email).Returns(user);
            _passwordProvider.CheckPassword(model.Password, user.Password).Returns(false);

            LoginResult result = _loginController.LoginPost(model) as LoginResult;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            _userRepo.Received(1).GetUserByEmailOrDefault(model.Email);
            _passwordProvider.Received(1).CheckPassword(model.Password, user.Password);
        }

        [Test]
        public void LoginPost_ValidLogin_Succeeds()
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = "test@test.com";
            model.Password = "password";

            UserEntity user = TestEntityHelper.CreateUserWithData();
            user.Email = model.Email;

            _userRepo.GetUserByEmailOrDefault(model.Email).Returns(user);
            _passwordProvider.CheckPassword(model.Password, user.Password).Returns(true);

            LoginResult result = _loginController.LoginPost(model) as LoginResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            _userRepo.Received(1).GetUserByEmailOrDefault(model.Email);
            _passwordProvider.Received(1).CheckPassword(model.Password, user.Password);
        }

        #endregion

    }
}
