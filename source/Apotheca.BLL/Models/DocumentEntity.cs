using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class DocumentEntity
    {
        public Guid? Id { get; internal set; }

        public DateTime? CreatedOn { get; set; }

    }
}
