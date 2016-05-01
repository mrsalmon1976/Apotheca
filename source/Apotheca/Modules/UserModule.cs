using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Authentication.Forms;
using Apotheca.Controllers;
using Apotheca.ViewModels.Login;
using Nancy.ModelBinding;

namespace Apotheca.Modules
{
    public class UserModule : NancyModule
    {
        public UserModule(IUserController userController)
        {
            Get[Actions.User.Setup] = x =>
            {
                return userController.HandleSetupGet(this);
            };

            Post[Actions.User.Setup] = x =>
            {
                return userController.HandleSetupPost(this);
            };

        }
    }
}
