using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Security;
using Nancy.Authentication.Forms;

namespace Apotheca.Modules
{
    public class ApothecaSecureFormModule : NancyModule
    {
        public ApothecaSecureFormModule(IUserMapper userMapper)
        {
            var formsAuthConfiguration = new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "~/login",
                UserMapper = userMapper,
            };
            FormsAuthentication.Enable(this, formsAuthConfiguration);
            this.RequiresAuthentication();
            //this.RequiresClaims(new[] { "Admin" });
            //Before += ctx =>
            //{
              //  return (this.Context.CurrentUser == null) ? new HtmlResponse(HttpStatusCode.Unauthorized) : null;
            //};

            // Your routes here
        }
    }
}
