using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.Document
{
    public class DocumentSearchViewModel : BaseViewModel
    {
        public DocumentSearchViewModel() : base()
        {
            this.Results = new List<DocumentSearchResult>();
        }

        public string SearchText { get; set; }

        public List<DocumentSearchResult> Results { get; private set; }

        public bool IsResultGridVisible { get; set; }


    }
}
