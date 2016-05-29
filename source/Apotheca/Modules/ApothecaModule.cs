using Apotheca.ViewModels;
using Apotheca.Web.Results;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Responses;
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

        protected void AddScript(string script)
        {
            ((List<string>)Context.ViewBag.Scripts).Add(script);
        }

        protected object HandleResult(IControllerResult result)
        {
            if (result == null) throw new ArgumentNullException("result");

            ViewResult vr = result as ViewResult;
            if (vr != null)
            {
                return this.View[vr.ViewName, vr.Model];
            }

            RedirectResult rr = result as RedirectResult;
            if (rr != null)
            {
                return this.Response.AsRedirect(rr.Location);
            }

            LoginAndRedirectResult lrr = result as LoginAndRedirectResult;
            if (lrr != null)
            {
                return this.LoginAndRedirect(lrr.UserId, DateTime.Now.AddDays(1), lrr.Location);
            }

            JsonResult jr = result as JsonResult;
            if (jr != null)
            {
                return new JsonResponse(jr.Model, new DefaultJsonSerializer());
            }

            TextResult tr = result as TextResult;
            if (tr != null)
            {
                return Response.AsText(tr.Text);
            }

            FileResult fr = result as FileResult;
            if (fr != null)
            {
                return Response.AsFile(fr.ApplicationRelativeFilePath, fr.ContentType);
            }

            NotFoundResult nfr = result as NotFoundResult;
            if (nfr != null)
            {
                return HttpStatusCode.NotFound;
            }

            throw new NotSupportedException("Results of type " + result.GetType().Name + " not supported");
        }

    }
}
