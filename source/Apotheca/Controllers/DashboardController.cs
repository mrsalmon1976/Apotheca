using Apotheca.BLL.Data;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Apotheca.Web.Results;
using System.Threading.Tasks;

namespace Apotheca.Controllers
{
    public interface IDashboardController
    {
        Task<IControllerResult> HandleDashboardGetAsync();
    }

    public class DashboardController : IDashboardController
    {
        private IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IControllerResult> HandleDashboardGetAsync()
        {

            var userCount = _unitOfWork.UserRepo.GetUserCountAsync();
            var docCount = _unitOfWork.DocumentRepo.GetCountAsync();
            var notifications = _unitOfWork.AuditLogRepo.GetLatest(10);

            await Task.WhenAll(userCount, docCount, notifications);

            DashboardViewModel model = new DashboardViewModel();
            model.UserCount = userCount.Result;
            model.DocumentCount = docCount.Result;
            model.Notifications.AddRange(notifications.Result);
            return new ViewResult(Views.Dashboard, model);

        }


    }
}
