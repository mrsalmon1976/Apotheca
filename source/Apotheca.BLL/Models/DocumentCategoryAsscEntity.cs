using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Models
{
    public class DocumentCategoryAsscEntity
    {
        public int Id { get; internal set; }

        public Guid DocumentId { get; set; }

        public int DocumentVersionNo { get; set; }

        public Guid CategoryId { get; set; }

    }
}
