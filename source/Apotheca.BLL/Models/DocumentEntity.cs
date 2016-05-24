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

        public string FileName { get; set; }

        public string Extension { get; set; }

        public string Description { get; set; }

        public byte[] FileContents { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        public string MimeType { get; set; }

    }
}
