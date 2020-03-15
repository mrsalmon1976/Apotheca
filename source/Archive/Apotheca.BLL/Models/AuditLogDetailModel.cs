using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class AuditLogDetailModel
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public string Key { get; set; }
        public string Action { get; set; }
        public DateTime AuditDateTime { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string EntityDetail { get; set; }

    }
}
