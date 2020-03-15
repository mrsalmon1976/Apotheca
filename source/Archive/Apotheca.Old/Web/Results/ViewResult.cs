using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class ViewResult : IControllerResult
    {
        public ViewResult(string viewName, object model)
        {
            this.ViewName = viewName;
            this.Model = model;
        }

        public string ViewName { get; set; }

        public object Model { get; set; }
    }
}
