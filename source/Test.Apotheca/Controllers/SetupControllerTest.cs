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
using Nancy.ModelBinding;
using Apotheca.ViewModels.Login;
using Apotheca.Web.Results;
using Apotheca.Content.Views;
using Apotheca.ViewModels.User;
using Apotheca.BLL.Commands.User;
using Apotheca.Validators;
using Test.Apotheca.TestHelpers;
using Apotheca.BLL.Models;
using Apotheca.BLL.Exceptions;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class SetupControllerTest
    {
        private ISetupController _setupController;
        private IUserRepository _userRepo;
        private ICreateUserCommand _createUserCommand;
        private IUserViewModelValidator _userViewModelValidator;

        [SetUp]
        public void SetupControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _createUserCommand = Substitute.For<ICreateUserCommand>();
            _userViewModelValidator = Substitute.For<IUserViewModelValidator>();

            _setupController = new SetupController(_userRepo, _createUserCommand, _userViewModelValidator);
        }

        [Test]
        public void DefaultGet_UsersExist_Redirects()
        {
            // setup 
            _userRepo.UsersExist().Returns(true);

            // execute
            RedirectResult result = _setupController.DefaultGet() as RedirectResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Actions.Login.Default, result.Location);
        }

        [Test]
        public void DefaultGet_NoUsersExist_DisplaysView()
        {
            // setup 
            _userRepo.UsersExist().Returns(false);

            // execute
            ViewResult result = _setupController.DefaultGet() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Setup.Default, result.ViewName);

            UserViewModel viewModel = result.Model as UserViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(Actions.Setup.Default, viewModel.FormAction);
        }

        [Test]
        public void DefaultPost_OnExecute_SetsModelValues()
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            model.Role = "";
            this._userViewModelValidator.Validate(model).Returns(new List<string>());

            _setupController.DefaultPost(model);

            Assert.AreEqual(Roles.Admin, model.Role);
            Assert.AreEqual(Actions.Setup.Default, model.FormAction);
        }

        [Test]
        public void DefaultPost_OnModelValidationFailure_ExitsToSetupView()
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            List<string> errors = new List<string>() { "error" };
            this._userViewModelValidator.Validate(model).Returns(errors);

            ViewResult result = _setupController.DefaultPost(model) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Setup.Default, result.ViewName);

            // command should not have been executed
            _createUserCommand.DidNotReceive().Execute();
        }

        [Test]
        public void DefaultPost_OnDbValidationFailure_ExitsToSetupView()
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            this._userViewModelValidator.Validate(model).Returns(new List<string>());

            List<string> errors = new List<string>() { "error1", "error2" };

            _createUserCommand.When(x => x.Execute()).Throw(new ValidationException(errors));

            // execute
            ViewResult result = _setupController.DefaultPost(model) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Setup.Default, result.ViewName);

            // command should have been executed
            _createUserCommand.Received(1).Execute();
            Assert.AreEqual(errors.Count, model.ValidationErrors.Count);
        }

        [Test]
        public void DefaultPost_OnValidationSuccess_RedirectsToDashboard()
        {
            // setup
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            this._userViewModelValidator.Validate(model).Returns(new List<string>());

            // execute
            LoginAndRedirectResult result = _setupController.DefaultPost(model) as LoginAndRedirectResult;

            // assert
            _createUserCommand.Received(1).Execute();
            Assert.IsNotNull(result);
            Assert.AreEqual(model.Id.Value, result.UserId);
            Assert.AreEqual(Actions.Dashboard.Default, result.Location);

        }

    }
}
