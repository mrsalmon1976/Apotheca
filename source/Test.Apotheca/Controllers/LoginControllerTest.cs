﻿using Apotheca.BLL.Repositories;
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
using Apotheca.Content.Views;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class LoginControllerTest
    {
        private ILoginController _loginController;
        private IUserRepository _userRepo;

        [SetUp]
        public void LoginControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _loginController = new LoginController(_userRepo);
        }

        [Test]
        public void HandleLoginGet_NoUsers_RedirectsToSetup()
        {
            // setup 
            _userRepo.UsersExist().Returns(false);

            // execute
            RedirectResult result = _loginController.LoginGet() as RedirectResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Actions.Setup.Default, result.Location);
            _userRepo.Received(1).UsersExist();

        }

        [Test]
        public void HandleLoginGet_UsersExist_ReturnsView()
        {
            // setup 
            _userRepo.UsersExist().Returns(true);

            // execute
            ViewResult result = _loginController.LoginGet() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Login.Default, result.ViewName);

            LoginViewModel model = result.Model as LoginViewModel;
            Assert.IsNotNull(model);

            _userRepo.Received(1).UsersExist();

        }

    }
}