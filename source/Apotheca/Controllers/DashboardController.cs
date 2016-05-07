using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Modules;
using Apotheca.ViewModels.Dashboard;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Responses.Negotiation;
using Apotheca.Web.Results;

namespace Apotheca.Controllers
{
    public interface IDashboardController
    {
        Task<IControllerResult> HandleDashboardGetAsync();
    }

    public class DashboardController : IDashboardController
    {
        private IUserRepository _userRepo;
        private IDocumentRepository _documentRepo;

        public DashboardController(IUserRepository userRepo, IDocumentRepository documentRepo)
        {
            _userRepo = userRepo;
            _documentRepo = documentRepo;
        }

        public async Task<IControllerResult> HandleDashboardGetAsync()
        {

            var userCount = _userRepo.GetUserCountAsync();
            var docCount = _documentRepo.GetCountAsync();

            await Task.WhenAll(userCount, docCount);

            DashboardViewModel model = new DashboardViewModel();
            model.UserCount = userCount.Result;
            model.DocumentCount = docCount.Result;
            return new ViewResult(Views.Dashboard, model);

        }


    }
}
