using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.ViewModels.Dashboard;
using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Modules
{
    public class DocumentModule : ApothecaSecureFormModule
    {
        public DocumentModule(IUserMapper userMapper, IDocumentController documentController) : base(userMapper)
        {
            Get[Actions.Document.Add, true] = async (x, ct) =>
            {
                return await documentController.HandleDocumentAddGetAsync(this);
            };
        }
    }
}
