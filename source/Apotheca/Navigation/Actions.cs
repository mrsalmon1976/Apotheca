using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Navigation
{
    public class Actions
    {
        public const string Dashboard = "/dashboard";

        public class Category
        {
            public const string Default = "/category";
        }

        public class Document
        {
            public const string Default = "/document";

            public const string Add = "/document/add";

            public const string Upload = "/document/upload";
        }

        public class Login
        {
            public const string Default = "/login";
        }

        public class Setup
        {
            public const string Default = "/setup";
        }
    }
}
