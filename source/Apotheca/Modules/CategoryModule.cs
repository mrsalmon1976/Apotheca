using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Apotheca.ViewModels.Document;
using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ModelBinding;
using Apotheca.BLL.Models;

namespace Apotheca.Modules
{
    public class CategoryModule : ApothecaSecureFormModule
    {
        public CategoryModule(IRootPathProvider pathProvider, ICategoryController categoryController) : base()//, documentController)
        {
            Get[Actions.Category.Default] = (x) =>
            {
                AddScript(Scripts.CategoryFormView);
                return this.HandleResult(categoryController.HandleCategoryGet());
            };
            Post[Actions.Category.Default] = (x) =>
            {
                return this.HandleResult(categoryController.HandleCategoryPost(this.Bind<CategoryEntity>()));
            };
        }
    }
}
