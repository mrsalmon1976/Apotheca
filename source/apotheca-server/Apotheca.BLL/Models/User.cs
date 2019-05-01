using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public string Token { get; set; }
    }
}
