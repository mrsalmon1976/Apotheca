using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Apotheca.ViewModels.Document;
using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ModelBinding;
using Apotheca.BLL.Models;
using Apotheca.ViewModels.User;
using Apotheca.BLL.Utils;

namespace Apotheca.Modules
{
    public class UserModule : ApothecaSecureFormModule
    {
        public UserModule(IRootPathProvider pathProvider, IUserController userController) : base()
        {
            Get[Actions.User.Default] = (x) =>
            {
                AddScript(Scripts.UserView);
                return this.HandleResult(userController.HandleUserGet(Request.Query["id"]));
            };
            Get[Actions.User.List] = (x) =>
            {
                return this.HandleResult(userController.HandleUserGetList());
            };
            Post[Actions.User.Default] = (x) =>
            {
                var model = this.Bind<UserViewModel>();
                return this.HandleResult(userController.HandleUserPost(model));
            };
        }
    }
}
