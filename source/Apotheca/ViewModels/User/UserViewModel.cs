using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.User
{
    public class UserViewModel : Apotheca.BLL.Models.User
    {
        public string ConfirmPassword { get; set; }

        public string FormAction { get; set; }

        /// <summary>
        /// Determines whether to show the permissions options (role, categories, etc).
        /// </summary>
        public bool IsPermissionPanelVisible { get; set; }
    }
}
