using Apotheca.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(Constants.MinimumPasswordLength)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
