using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Apotheca.Controllers;

namespace Apotheca.Modules
{
    public class ApothecaSecureFormModule : ApothecaModule
    {
        public ApothecaSecureFormModule()//, IController controller)
        {
            this.RequiresAuthentication();
            //controller.CurrentUser = this.Context.CurrentUser;
            
            //this.RequiresClaims(new[] { "Admin" });
            //Before += ctx =>
            //{
              //  return (this.Context.CurrentUser == null) ? new HtmlResponse(HttpStatusCode.Unauthorized) : null;
            //};

            // Your routes here
        }

    }
}
