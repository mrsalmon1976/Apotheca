using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public static class Roles
    {
        static Roles()
        {
            AllRoles = new List<string>(new string[] { Admin, Moderator, User }).AsReadOnly();
        }

        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string User = "User";

        public static IReadOnlyList<string> AllRoles { get; private set; }
    }
}
