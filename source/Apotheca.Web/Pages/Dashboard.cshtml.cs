using Apotheca.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.Pages
{
    public class DashboardModel : DefaultViewModel
    {
        public DashboardModel()
        {
            this.RecentItems = new List<DashboardModelItem>();
        }

        public List<DashboardModelItem> RecentItems { get; private set; }

        public class DashboardModelItem
        {

        }
    }
}
