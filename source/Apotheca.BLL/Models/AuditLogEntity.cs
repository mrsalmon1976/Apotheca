using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class AuditLogEntity
    {
        public class Actions
        {
            public const string Insert = "INSERT";
            public const string Update = "UPDATE";
            public const string Delete = "DELETE";
        }

        public int Id { get; set; }
		public string Entity { get; set; }
        public string Key { get; set; }
        public string Action { get; set; }
        public DateTime AuditDateTime { get; set; }
		public Guid UserId { get; set; }
        public string EntityDetail { get; set; }

        public static AuditLogEntity Create(string entity, object key, string action, Guid userId, object detail)
        {
            AuditLogEntity logEntity = new AuditLogEntity()
            {
                Action = action,
                Entity = entity,
                Key = (key == null ? null : key.ToString()),
                UserId = userId,
                EntityDetail = (detail == null ? String.Empty : JsonConvert.SerializeObject(detail))
            };
            return logEntity;
        }


    }
}
