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

namespace Apotheca.Modules
{
    public class DocumentModule : ApothecaSecureFormModule
    {
        public DocumentModule(IRootPathProvider pathProvider, IDocumentController documentController) : base()//, documentController)
        {
            Get[Actions.Document.Add] = (x) =>
            {
                AddScript(Scripts.DocumentFormView);
                return this.HandleResult(documentController.HandleDocumentAddGet());
            };

            Post[Actions.Document.Add] = (x) =>
            {
                return base.HandleResult(documentController.HandleDocumentAddPost(pathProvider.GetRootPath(), this.Context.CurrentUser.UserName, this.Bind<DocumentViewModel>()));
            };

            Get[Actions.Document.Search] = (x) =>
            {
                //AddScript(Scripts.DocumentFormView);
                return this.HandleResult(documentController.HandleDocumentSearchGet());
            };

            Post[Actions.Document.Search] = (x) =>
            {
                return base.HandleResult(documentController.HandleDocumentSearchPost(this.Bind<DocumentSearchViewModel>()));
            };

            Post[Actions.Document.Upload] = x =>
            {
                documentController.HandleDocumentUploadPost(pathProvider.GetRootPath(), Request.Files);
                return Response.AsText("");
            };
        }
    }
}
