using Apotheca.BLL.Data;
using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.ViewModels.Dashboard;
using Apotheca.Web.Results;
using NSubstitute;
using NUnit.Framework;
using Test.Apotheca.TestHelpers;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class DashboardControllerTest
    {
        private IDashboardController _dashboardController;
        private IUserRepository _userRepo;
        private IUnitOfWork _unitOfWork;

        private IDocumentRepository _documentRepo;

        [SetUp]
        public void DashboardControllerTest_SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _documentRepo = Substitute.For<IDocumentRepository>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.DocumentRepo.Returns(_documentRepo);
            _unitOfWork.UserRepo.Returns(_userRepo);
            _dashboardController = new DashboardController(_unitOfWork);

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
