using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.UtcNow;
            this.Stores = new List<Guid>();
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public string Token { get; set; }

        public DateTime Created { get; set; }

        public List<Guid> Stores { get; set; }

        public DateTime? RegistrationCompleted { get; set; }

    }
}
