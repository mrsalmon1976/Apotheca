using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class AuditLogDetailEntity
    {
        public int Id { get; set; }
		public int AuditLogId { get; set; }
		public string Column { get; set; }
		public string FromValue { get; set; }
		public string ToValue { get; set; }

    }
}
