using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.User
{
    public class UserViewModel : Apotheca.BLL.Models.UserEntity
    {
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets/sets the form action for the user form.
        /// </summary>
        public string FormAction { get; set; }

        /// <summary>
        /// Determines whether to show the permissions options (role, categories, etc).
        /// </summary>
        public bool IsPermissionPanelVisible { get; set; }
    }
}
