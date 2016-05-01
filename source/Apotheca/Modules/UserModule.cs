using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Authentication.Forms;
using Apotheca.Controllers;
using Apotheca.ViewModels.Login;

namespace Apotheca.Modules
{
    public class UserModule : NancyModule
    {
        public UserModule(IUserController userController)
        {
            Get["/users/seed"] = x =>
            {
                return userController.HandleSeedGet(this);
            };

        }
    }
}
