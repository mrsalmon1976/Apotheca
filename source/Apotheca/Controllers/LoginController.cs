using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Modules;
using Apotheca.ViewModels.Login;
using Apotheca.Web.Results;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apotheca.Web;
using Nancy.Security;
using Apotheca.BLL.Data;

namespace Apotheca.Controllers
{
    public interface ILoginController
    {
        IControllerResult LoginGet(IUserIdentity currentUser, LoginViewModel model);
    }

    public class LoginController : ILoginController
    {
        private IUnitOfWork _unitOfWork;

        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IControllerResult LoginGet(IUserIdentity currentUser, LoginViewModel model)
        {
            if (!_unitOfWork.UserRepo.UsersExist())
            {
                return new RedirectResult(Actions.Setup.Default);
            }

            if (currentUser != null)
            {
                return new RedirectResult(Actions.Dashboard);
            }
            
            return new ViewResult(Views.Login, model);
        }
    }
}
