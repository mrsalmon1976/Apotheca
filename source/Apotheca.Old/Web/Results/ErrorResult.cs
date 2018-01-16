using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class ErrorResult : IControllerResult
    {
        public ErrorResult(HttpStatusCode statusCode)
        {
            this.HttpStatusCode = statusCode;
        }

        public HttpStatusCode HttpStatusCode { get; set; }

    }
}
