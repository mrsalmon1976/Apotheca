using Apotheca.BLL.Repositories;
using Apotheca.Content.Views;
using Apotheca.Modules;
using Apotheca.ViewModels.Login;
using Apotheca.Web.Results;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Controllers
{
    public interface ILoginController
    {
        IControllerResult LoginGet();
    }

    public class LoginController : ILoginController
    {
        private IUserRepository _userRepo;

        public LoginController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public IControllerResult LoginGet()
        {
            if (!_userRepo.UsersExist())
            {
                return new RedirectResult(Actions.Setup.Default);
            }

            LoginViewModel model = new LoginViewModel();
            return new ViewResult(Views.Login.Default, model);
        }
    }
}
