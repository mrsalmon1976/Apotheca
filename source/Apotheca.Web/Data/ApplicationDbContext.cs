using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Apotheca.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //builder.Entity<IdentityUser>().ToTable("Users", "identity");
            builder.Entity<ApplicationUser>().ToTable("Users", "identity");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "identity");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "identity");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "identity");
            builder.Entity<ApplicationRole>().ToTable("Roles", "identity");
        }
    }
}
