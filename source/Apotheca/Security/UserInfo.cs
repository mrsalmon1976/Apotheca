using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Security
{
    public interface IUserInfo : IUserIdentity
    {
    }

    public class UserInfo : IUserInfo
    {
        public System.Collections.Generic.IEnumerable<string> Claims
        {
            get { return new string[] { }; }
        }

        public string UserName
        {
            get { return "Test"; }
        }
    }
}
