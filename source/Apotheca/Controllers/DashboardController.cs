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

namespace Apotheca.Controllers
{
    public interface IDashboardController
    {
        Task<object> HandleDashboardGetAsync(DashboardModule module);
    }

    public class DashboardController : IDashboardController
    {
        private IUserRepository _userRepo;

        public DashboardController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<object> HandleDashboardGetAsync(DashboardModule module)
        {

            var userCount = _userRepo.GetUserCountAsync();

            await Task.WhenAll(userCount);

            DashboardViewModel model = new DashboardViewModel();
            model.UserCount = userCount.Result;
            return module.View[Views.Dashboard, model];

        }


    }
}
