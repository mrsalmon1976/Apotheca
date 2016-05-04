using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Modules;
using Apotheca.ViewModels.Dashboard;
using Apotheca.ViewModels.Document;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Controllers
{
    public interface IDocumentController
    {
        Task<object> HandleDocumentAddGetAsync(INancyModule module);
    }

    public class DocumentController : IDocumentController
    {
        public DocumentController()
        {
        }

        public async Task<object> HandleDocumentAddGetAsync(INancyModule module)
        {

            //var userCount = _userRepo.GetUserCountAsync();

            //await Task.WhenAll(userCount);

            DocumentViewModel model = new DocumentViewModel();
            return module.View[Views.Document.Add, model];

        }


    }
}
