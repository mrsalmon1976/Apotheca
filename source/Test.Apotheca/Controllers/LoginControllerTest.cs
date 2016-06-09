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

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class LoginControllerTest
    {
        private ILoginController _loginController;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepo;

        [SetUp]
        public void LoginControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.UserRepo.Returns(_userRepo);

            _loginController = new LoginController(_unitOfWork);
        }

        [Test]
        public void HandleLoginGet_NoUsers_RedirectsToSetup()
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
        public void HandleLoginGet_AlreadyLoggedIn_RedirectsToDashboard()
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
        public void HandleLoginGet_UsersExist_ReturnsView()
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

    }
}
