using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Security
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity()
        {
            this.Claims = Enumerable.Empty<string>();
        }

        public IEnumerable<string> Claims { get; set; }

        public string UserName { get; set; }

    }
}
