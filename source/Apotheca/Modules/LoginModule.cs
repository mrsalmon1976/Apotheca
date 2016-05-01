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
    public class LoginModule : NancyModule
    {
        public LoginModule(ILoginController loginController)
        {
            Get["/"] = x =>
            {
                return this.Response.AsRedirect("/login");
            };

            Get["/login"] = x =>
            {

                LoginViewModel model = null;// loginController.GetLoginViewData();
                //if (model.UsersExist)
                //{
                    //return View["Content/Views/LoginView.cshtml", model];
                //}
                return loginController.HandleLoginGet(this);
            };

            Post["/login"] = x =>
            {
                // TODO: Complete authentication
                return this.LoginAndRedirect(Guid.NewGuid(), DateTime.Now.AddDays(1), "/dashboard");
                //return this.Response.AsRedirect("/dashboard");
            };

            Get["/logout"] = x =>
            {
                return this.Logout("/login");
            };

        }
    }
}
