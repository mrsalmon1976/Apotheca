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

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class SetupControllerTest
    {
        private ISetupController _setupController;
        private IUserRepository _userRepo;

        [SetUp]
        public void SetupControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _setupController = new SetupController(_userRepo);
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


    }
}
