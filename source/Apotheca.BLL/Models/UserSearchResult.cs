using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class UserSearchResult
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, Surname).Trim();
            }
        }

        public string Role { get; set; }

        public string ApiKey { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
