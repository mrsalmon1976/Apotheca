using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.User
{
    public class UserViewModel : UserEntity
    {
        public UserViewModel()
        {
            this.ValidationErrors = new List<string>();
        }

        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Determines whether to show the permissions options (role, categories, etc).
        /// </summary>
        public bool IsPermissionPanelVisible { get; set; }

        public List<string> ValidationErrors { get; private set; }

    }
}
