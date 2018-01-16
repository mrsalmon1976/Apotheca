using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.Data
{
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
