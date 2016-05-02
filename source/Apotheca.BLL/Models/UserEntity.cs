using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class UserEntity
    {
        public Guid? Id { get; internal set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Role { get; set; }

        public string ApiKey { get; internal set; }

        public DateTime? CreatedOn { get; set; }

    }
}
