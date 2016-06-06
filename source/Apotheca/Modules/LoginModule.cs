using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Apotheca.Controllers;
using Apotheca.ViewModels.Login;
using Apotheca.Navigation;

namespace Apotheca.Modules
{
    public class LoginModule : ApothecaModule
    {
        public LoginModule(ILoginController loginController)
        {
            Get["/"] = x =>
            {
                return this.Response.AsRedirect(Actions.Login.Default);
            };

            Get["/login"] = x =>
            {
                var model = this.Bind<LoginViewModel>();
                return this.HandleResult(loginController.LoginGet(this.Context.CurrentUser, model));
            };

            Post["/login"] = x =>
            {
                // TODO: Complete authentication
                // TODO: Move logic into controller
                var model = this.Bind<LoginViewModel>();
                if (String.IsNullOrEmpty(model.ReturnUrl)) model.ReturnUrl = Actions.Dashboard;
                return this.LoginAndRedirect(new Guid("58DCBB23-2C2B-E611-BFE4-506313A3F1A1"), DateTime.Now.AddDays(1), model.ReturnUrl);
            };

            Get["/logout"] = x =>
            {
                return this.Logout(Actions.Login.Default);
            };

        }
    }
}
