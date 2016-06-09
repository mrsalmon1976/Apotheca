using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.ViewModels.Category
{
    public class CategoryListViewModel : BaseViewModel
    {
        public CategoryListViewModel()
            : base()
        {
            this.Categories = new List<CategorySearchResult>();
        }

        public List<CategorySearchResult> Categories { get; private set; }
        

    }
}
