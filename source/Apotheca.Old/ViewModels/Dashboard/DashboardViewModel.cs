using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.Dashboard
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
        {
            this.Notifications = new List<AuditLogDetailModel>();
        }

        public int UserCount { get; set; }

        public int DocumentCount { get; set; }

        public List<AuditLogDetailModel> Notifications { get; private set; }
    }
}
