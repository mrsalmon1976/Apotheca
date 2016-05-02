using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Web.Results
{
    public class LoginAndRedirectResult : IControllerResult
    {
        public LoginAndRedirectResult(Guid userId, string location)
        {
            this.UserId = userId;
            this.Location = location;
        }

        public string Location { get; set; }

        public Guid UserId { get; set; }
    }
}
