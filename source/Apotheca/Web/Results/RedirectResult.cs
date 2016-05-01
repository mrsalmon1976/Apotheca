using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class RedirectResult : IControllerResult
    {
        public RedirectResult(string location)
        {
            this.Location = location;
        }

        public string Location { get; set; }
    }
}
