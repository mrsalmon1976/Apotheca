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
using SystemWrapper.IO;
using System.Reflection;
using System.IO;
using Apotheca.ViewModels.Dashboard;
using Test.Apotheca.TestHelpers;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class DashboardControllerTest
    {
        private IDashboardController _dashboardController;
        private IUserRepository _userRepo;

        private IDocumentRepository _documentRepo;

        [SetUp]
        public void DashboardControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _documentRepo = Substitute.For<IDocumentRepository>();
            _dashboardController = new DashboardController(_userRepo, _documentRepo);

        }

        [Test]
        public async void HandleDashboardGetAsync_OnExecute_SetsModelValues()
        {
            // setup
            int userCount = TestRandomHelper.GetInt(5, 1000000);
            int docCount = TestRandomHelper.GetInt(5, 1000000);

            _userRepo.GetUserCountAsync().Returns(userCount);
            _documentRepo.GetCountAsync().Returns(docCount);


            // execute
            ViewResult result = await _dashboardController.HandleDashboardGetAsync() as ViewResult;
            DashboardViewModel resultModel = result.Model as DashboardViewModel;

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultModel);

            Assert.AreEqual(userCount, resultModel.UserCount);
            Assert.AreEqual(docCount, resultModel.DocumentCount);
        }

    }
}
