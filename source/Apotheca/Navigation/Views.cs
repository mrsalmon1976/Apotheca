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
            public const string Form = "Content/Views/Categories/CategoryFormView.cshtml";
        }

        public const string Dashboard = "Content/Views/DashboardView.cshtml";

        public const string Login = "Content/Views/LoginView.cshtml";

        public class Document
        {
            public const string Add = "Content/Views/Document/DocumentFormView.cshtml";
            public const string Search = "Content/Views/Document/DocumentSearchView.cshtml";
        }

        public class Setup
        {
            public const string Default = "Content/Views/Setup/SetupView.cshtml";
        }
    }
}
