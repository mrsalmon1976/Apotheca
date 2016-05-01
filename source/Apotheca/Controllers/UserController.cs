using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Content.Views;
using Apotheca.Modules;
using Apotheca.ViewModels.Login;
using Apotheca.ViewModels.User;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Controllers
{
    public interface IUserController
    {
        object HandleSetupGet(UserModule module);

        object HandleSetupPost(UserModule module);

    }

    public class UserController : IUserController
    {
        private IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public object HandleSetupGet(UserModule module)
        {
            if (_userRepo.UsersExist())
            {
                return module.Response.AsRedirect(Actions.Login.Default);
            }

            UserViewModel model = new UserViewModel();
            model.FormAction = Actions.User.Setup;
            return module.View[Views.User.Setup, model];

        }

        public object HandleSetupPost(UserModule module)
        {
            UserViewModel model = module.Bind<UserViewModel>();
            model.Role = Roles.Admin;
            return module.View[Views.User.Setup, model];
        }
    }
}
