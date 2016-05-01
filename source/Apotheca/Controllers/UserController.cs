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
    public interface IUserController
    {
        object HandleSeedGet(UserModule module);
    }

    public class UserController : IUserController
    {
        private IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public object HandleSeedGet(UserModule module)
        {
            if (_userRepo.UsersExist())
            {
                return module.Response.AsRedirect("/login");
            }

            LoginViewModel model = new LoginViewModel();
            return module.View["Content/Views/User/SeedView.cshtml", model];

        }
    }
}
