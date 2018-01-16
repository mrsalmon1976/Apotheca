using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class JsonResult : IControllerResult
    {
        public JsonResult(object model)
        {
            this.Model = model;
        }

        public object Model { get; set; }
    }
}
