using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.Document
{
    public class DocumentViewModel //: UserEntity
    {
        public DocumentViewModel()
        {
            this.ValidationErrors = new List<string>();
        }

        public string DocumentName { get; set; }

        public string DocumentDetails { get; set; }
        ///// <summary>
        ///// Gets/sets the form action for the user form.
        ///// </summary>
        //public string FormAction { get; set; }

        ///// <summary>
        ///// Determines whether to show the permissions options (role, categories, etc).
        ///// </summary>
        //public bool IsPermissionPanelVisible { get; set; }

        public List<string> ValidationErrors { get; private set; }

        
    }
}
