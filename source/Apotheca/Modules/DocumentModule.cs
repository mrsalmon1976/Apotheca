﻿using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Modules
{
    public class DocumentModule : ApothecaSecureFormModule
    {
        public DocumentModule(IRootPathProvider pathProvider, IUserMapper userMapper, IDocumentController documentController) : base(userMapper)//, documentController)
        {
            Get[Actions.Document.Add, true] = async (x, ct) =>
            {
                AddScript("/Content/Js/Views/DocumentFormView.js");
                return await documentController.HandleDocumentAddGetAsync(this);
            };

            Post[Actions.Document.Upload] = x =>
            {
                documentController.HandleDocumentUploadPost(pathProvider.GetRootPath(), Request.Files);
                return Response.AsText("");
            };
        }
    }
}
