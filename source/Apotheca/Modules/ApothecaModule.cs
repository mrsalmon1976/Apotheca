using Apotheca.ViewModels;
using Apotheca.Web.Results;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Modules
{
    public class ApothecaModule : NancyModule
    {
        public ApothecaModule()
        {
        }

        protected object HandleResult(IControllerResult result)
        {
            if (result == null) throw new ArgumentNullException("result");

            RedirectResult rr = result as RedirectResult;
            if (rr != null)
            {
                return this.Response.AsRedirect(rr.Location);
            }

            ViewResult vr = result as ViewResult;
            if (vr != null)
            {
                return this.View[vr.ViewName, vr.Model];
            }

            throw new NotSupportedException("Results of type " + result.GetType().Name + " not supported");
        }

    }
}
