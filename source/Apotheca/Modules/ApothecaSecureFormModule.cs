using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Apotheca.Controllers;
using Apotheca.BLL.Models;

namespace Apotheca.Modules
{
    public class ApothecaSecureFormModule : ApothecaModule
    {
        public ApothecaSecureFormModule()
        {
            this.RequiresAuthentication();
            
            //this.RequiresClaims(new[] { "Admin" });
            //this.Before += ctx =>
            //{
            //    return (this.Context.CurrentUser == null) ? new HtmlResponse(HttpStatusCode.Unauthorized) : null;
            //};

            // Your routes here
        }

    }
}
