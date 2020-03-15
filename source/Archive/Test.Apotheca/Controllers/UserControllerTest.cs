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
using Apotheca.Navigation;
using Apotheca.ViewModels.User;
using Apotheca.BLL.Commands.User;
using Apotheca.Validators;
using Test.Apotheca.TestHelpers;
using Apotheca.BLL.Models;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Data;
using Apotheca.ViewModels;
using Test.Apotheca.BLL.TestHelpers;
using Apotheca.Security;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private IUserController _userController;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepo;
        private ICategoryRepository _categoryRepo;
        private ISaveUserCommand _createUserCommand;
        private IUserViewModelValidator _userViewModelValidator;

        [SetUp]
        public void UserControllerTest_User()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _categoryRepo = Substitute.For<ICategoryRepository>();
            _createUserCommand = Substitute.For<ISaveUserCommand>();
            _userViewModelValidator = Substitute.For<IUserViewModelValidator>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.UserRepo.Returns(_userRepo);
            _unitOfWork.CategoryRepo.Returns(_categoryRepo);
            _userController = new UserController(_unitOfWork, _createUserCommand, _userViewModelValidator);
        }

        [Test]
        public void HandleUserGet_OnExecute_SetsUpModel()
        {
            Guid userId = Guid.NewGuid();
            CategoryEntity[] categories = { TestEntityHelper.CreateCategoryWithData(), TestEntityHelper.CreateCategoryWithData(), TestEntityHelper.CreateCategoryWithData() };
            _categoryRepo.GetAll().Returns(categories);

            // execute
            ViewResult result = _userController.HandleUserGet(userId) as ViewResult;
            Assert.IsNotNull(result);

            UserViewModel viewModel = result.Model as UserViewModel;
            Assert.IsNotNull(viewModel);

            // check mode values
            Assert.AreEqual(Roles.AllRoles, viewModel.Roles);
            Assert.IsTrue(viewModel.IsPermissionPanelVisible);
            Assert.AreEqual(Roles.User, viewModel.SelectedRole);
            Assert.AreEqual(categories.Length, viewModel.CategoryOptions.Count);

            _categoryRepo.Received(1).GetAll();
        }

        [Test]
        public void HandleUserPost_OnModelValidationFailure_ExitsToUserView()
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            List<string> errors = new List<string>() { "error" };
            this._userViewModelValidator.Validate(model).Returns(errors);
            UserIdentity currentUser = new UserIdentity() { Id = Guid.NewGuid() };

            JsonResult result = _userController.HandleUserPost(model, currentUser) as JsonResult;
            Assert.IsNotNull(result);
            
            BasicResult basicResult = result.Model as BasicResult;
            Assert.IsNotNull(basicResult);

            // command should not have been executed
            _createUserCommand.DidNotReceive().Execute();
        }

        [Test]
        public void HandleUserPost_OnDbValidationFailure_ExitsToUserView()
        {
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            this._userViewModelValidator.Validate(model).Returns(new List<string>());
            UserIdentity currentUser = new UserIdentity() { Id = Guid.NewGuid() };

            List<string> errors = new List<string>() { "error1", "error2" };

            _createUserCommand.When(x => x.Execute()).Throw(new ValidationException(errors));

            // execute
            JsonResult result = _userController.HandleUserPost(model, currentUser) as JsonResult;
            Assert.IsNotNull(result);

            BasicResult basicResult = result.Model as BasicResult;
            Assert.IsNotNull(basicResult);

            // command should have been executed
            _createUserCommand.Received(1).Execute();
            Assert.AreEqual(errors.Count, basicResult.Messages.Length);
        }

        [Test]
        public void HandleUserPost_OnValidationSuccess_ReturnsSuccess()
        {
            // User
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            this._userViewModelValidator.Validate(model).Returns(new List<string>());
            UserIdentity currentUser = new UserIdentity() { Id = Guid.NewGuid() };

            // execute
            JsonResult result = _userController.HandleUserPost(model, currentUser) as JsonResult;
            Assert.IsNotNull(result);

            BasicResult basicResult = result.Model as BasicResult;
            Assert.IsNotNull(basicResult);

            // assert
            _createUserCommand.Received(1).Execute();
            _createUserCommand.Received(1).CategoryIds = model.CategoryIds;
            _createUserCommand.Received(1).CurrentUserId = currentUser.Id;
            _createUserCommand.Received(1).User = Arg.Any<UserEntity>();
            Assert.AreEqual(0, basicResult.Messages.Length);
            Assert.IsTrue(basicResult.Success);

        }

        [Test]
        public void HandleUserPost_OnSuccess_UsesTransaction()
        {
            // User
            UserViewModel model = TestViewModelHelper.CreateUserViewModelWithData();
            this._userViewModelValidator.Validate(model).Returns(new List<string>());
            UserIdentity currentUser = new UserIdentity() { Id = Guid.NewGuid() };

            // execute
            _userController.HandleUserPost(model, currentUser);

            // assert
            _unitOfWork.Received(1).BeginTransaction();
            _unitOfWork.Received(1).Commit();

        }

    }
}
