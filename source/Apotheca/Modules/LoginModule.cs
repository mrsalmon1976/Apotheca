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

                return this.HandleResult(loginController.LoginGet());
            };

            Post["/login"] = x =>
            {
                // TODO: Complete authentication
                return this.LoginAndRedirect(Guid.NewGuid(), DateTime.Now.AddDays(1), Actions.Dashboard);
            };

            Get["/logout"] = x =>
            {
                return this.Logout(Actions.Login.Default);
            };

        }
    }
}
