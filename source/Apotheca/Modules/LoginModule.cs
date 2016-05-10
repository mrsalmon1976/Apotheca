using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Authentication.Forms;
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
                return this.HandleResult(loginController.LoginGet(this.Context.CurrentUser));
            };

            Post["/login"] = x =>
            {
                // TODO: Complete authentication
                return this.LoginAndRedirect(new Guid("0E36B343-9A10-E611-BFE1-506313A3F1A1"), DateTime.Now.AddDays(1), Actions.Dashboard);
            };

            Get["/logout"] = x =>
            {
                return this.Logout(Actions.Login.Default);
            };

        }
    }
}
