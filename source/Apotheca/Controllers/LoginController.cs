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

namespace Apotheca.Controllers
{
    public interface ILoginController
    {
        IControllerResult LoginGet(IUserIdentity currentUser);
    }

    public class LoginController : ILoginController
    {
        private IUserRepository _userRepo;

        public LoginController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public IControllerResult LoginGet(IUserIdentity currentUser)
        {
            if (!_userRepo.UsersExist())
            {
                return new RedirectResult(Actions.Setup.Default);
            }

            if (currentUser != null)
            {
                return new RedirectResult(Actions.Dashboard);
            }
            
            LoginViewModel model = new LoginViewModel();
            return new ViewResult(Views.Login, model);
        }
    }
}
