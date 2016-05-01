using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Content.Views;
using Apotheca.Modules;
using Apotheca.ViewModels.Login;
using Apotheca.ViewModels.User;
using Apotheca.Web.Results;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Controllers
{
    public interface ISetupController
    {
        IControllerResult DefaultGet();

        IControllerResult DefaultPost(INancyModule module);

    }

    public class SetupController : ISetupController
    {
        private IUserRepository _userRepo;

        public SetupController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public IControllerResult DefaultGet()
        {
            if (_userRepo.UsersExist())
            {
                return new RedirectResult(Actions.Login.Default);
            }

            UserViewModel model = new UserViewModel();
            model.FormAction = Actions.Setup.Default;
            return new ViewResult(Views.Setup.Default, model);

        }

        public IControllerResult DefaultPost(INancyModule module)
        {
            UserViewModel model = module.Bind<UserViewModel>();
            model.Role = Roles.Admin;
            return new ViewResult(Views.Setup.Default, model);
        }
    }
}
