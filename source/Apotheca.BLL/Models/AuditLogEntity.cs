using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class AuditLogEntity
    {
        public int Id { get; set; }
		public string Action { get; set; }
		public string Table { get; set; }
		public DateTime AuditDateTime { get; set; }
		public Guid UserId { get; set; }

        public IEnumerable<AuditLogDetailEntity> AuditLogDetails { get; set; }

    }
}
