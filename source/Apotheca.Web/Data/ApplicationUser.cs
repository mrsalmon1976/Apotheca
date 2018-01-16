using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Apotheca.Web.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public ApplicationUser() : base()
        {
        }

        public ApplicationUser(string userName, string email) : base(userName, email)
        {
        }

        public string FullName { get; set; }
    }
}
