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
using Apotheca.BLL.Utils;

namespace Apotheca.Modules
{
    public class DocumentModule : ApothecaSecureFormModule
    {
        public DocumentModule(IRootPathProvider pathProvider, IDocumentController documentController) : base()
        {
            Get[Actions.Document.Add] = (x) =>
            {
                AddScript(Scripts.DocumentFormView);
                return this.HandleResult(documentController.HandleDocumentAddGet());
            };

            Post[Actions.Document.Add] = (x) =>
            {
                var model = this.Bind<DocumentViewModel>();
                return base.HandleResult(documentController.HandleDocumentFormPost(pathProvider.GetRootPath(), this.Context.CurrentUser, model));
            };

            Get[Actions.Document.Download] = (x) =>
            {
                Guid id = Request.Query["id"];
                return base.HandleResult(documentController.HandleDocumentDownloadGet(pathProvider.GetRootPath(), id));
            };

            Get[Actions.Document.Search] = (x) =>
            {
                string searchText = Request.Query["q"];
                return this.HandleResult(documentController.HandleDocumentSearchGet(searchText));
            };

            Get[Actions.Document.Update] = (x) =>
            {
                AddScript(Scripts.DocumentFormView);
                string id = Request.Query["id"];
                return this.HandleResult(documentController.HandleDocumentUpdateGet(id));
            };

            Post[Actions.Document.Update] = (x) =>
            {
                var model = this.Bind<DocumentViewModel>();
                return base.HandleResult(documentController.HandleDocumentFormPost(pathProvider.GetRootPath(), this.Context.CurrentUser, model));
            };
            Post[Actions.Document.Upload] = x =>
            {
                documentController.HandleDocumentUploadPost(pathProvider.GetRootPath(), Request.Files);
                return Response.AsText("");
            };
        }
    }
}
