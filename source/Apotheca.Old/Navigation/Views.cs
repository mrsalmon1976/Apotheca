using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Navigation
{
    public class Views
    {
        public class Category
        {
            public const string ListPartial = "Content/Views/Categories/_CategoryList.cshtml";
            public const string Default = "Content/Views/Categories/CategoryView.cshtml";
        }

        public const string Dashboard = "Content/Views/DashboardView.cshtml";

        public const string Login = "Content/Views/LoginView.cshtml";

        public class Document
        {
            public const string Add = "Content/Views/Document/DocumentFormView.cshtml";
            public const string Search = "Content/Views/Document/DocumentSearchView.cshtml";
            public const string Update = "Content/Views/Document/DocumentFormView.cshtml";
        }

        public class Setup
        {
            public const string Default = "Content/Views/Setup/SetupView.cshtml";
        }
        public class User
        {
            public const string ListPartial = "Content/Views/User/_UserList.cshtml";
            public const string Default = "Content/Views/User/UserView.cshtml";
        }
    }
}
