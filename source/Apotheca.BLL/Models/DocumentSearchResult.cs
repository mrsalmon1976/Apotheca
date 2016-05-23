using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class DocumentSearchResult
    {
        public Guid DocumentId { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}
