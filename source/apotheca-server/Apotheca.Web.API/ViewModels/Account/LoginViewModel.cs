using Apotheca.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]  // this isn't policy, but does force them to enter something in before we go off to AWS
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
