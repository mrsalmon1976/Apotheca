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
using Nancy.Security;
using Apotheca.BLL.Models;

namespace Apotheca.Modules
{
    public class CategoryModule : ApothecaSecureFormModule
    {
        public CategoryModule(IRootPathProvider pathProvider, ICategoryController categoryController) : base()//, documentController)
        {
            this.RequiresClaims(new[] { Roles.Admin });

            Get[Actions.Category.Default] = (x) =>
            {
                AddScript(Scripts.CategoryView);
                return this.HandleResult(categoryController.HandleCategoryGet());
            };
            Get[Actions.Category.List] = (x) =>
            {
                return this.HandleResult(categoryController.HandleCategoryGetList());
            };
            Post[Actions.Category.Default] = (x) =>
            {
                return this.HandleResult(categoryController.HandleCategoryPost(this.Bind<CategoryEntity>()));
            };
        }
    }
}
