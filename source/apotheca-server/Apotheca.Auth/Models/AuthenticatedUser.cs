using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.Auth.Models
{
    public class AuthenticatedUser
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
