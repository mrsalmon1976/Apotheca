using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.Document
{
    public class DocumentViewModel : BaseViewModel
    {
        public DocumentViewModel() : base()
        {
        }

        /// <summary>
        /// The document id - this will be null when adding a new document.
        /// </summary>
        public Guid? DocumentId { get; set; }

        public string FileName { get; set; }

        public string Extension
        {
            get
            {
                if (this.FileName == null) return null;
                return Path.GetExtension(this.FileName);
            }
        }

        public string Description { get; set; }

        public string UploadedFileName { get; set; }

    }
}
