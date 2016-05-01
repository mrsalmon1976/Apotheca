using Apotheca.BLL.Repositories;
using Apotheca.Modules;
using Apotheca.ViewModels.Login;
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
        object HandleLoginGet(LoginModule module);
    }

    public class LoginController : ILoginController
    {
        private IUserRepository _userRepo;

        public LoginController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public object HandleLoginGet(LoginModule module)
        {
            if (!_userRepo.UsersExist())
            {
                return module.Response.AsRedirect(Actions.User.Setup);
            }

            LoginViewModel model = new LoginViewModel();
            return module.View["Content/Views/LoginView.cshtml", model];

        }
    }
}
