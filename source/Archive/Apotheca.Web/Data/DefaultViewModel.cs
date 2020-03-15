using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Apotheca.Web.Data
{
    public class DefaultViewModel : PageModel
    {
        public string CurrentUserName
        {
            get
            {
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    return User.Identity.Name;
                }
                return null;
            }
        }

        public string AppVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            }
        }
    }
}
