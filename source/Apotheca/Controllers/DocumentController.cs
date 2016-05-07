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
using Nancy.Responses.Negotiation;
using System.IO;
using SystemWrapper.IO;

namespace Apotheca.Controllers
{
    public interface IDocumentController
    {
        Task<Negotiator> HandleDocumentAddGetAsync(INancyModule module);

        void HandleDocumentUploadPost(string rootPath, IEnumerable<HttpFile> files);
    }

    public class DocumentController : IDocumentController
    {
        private IPathHelper _pathHelper;
        private IDirectoryWrap _directoryWrap;
        private IPathWrap _pathWrap;

        public DocumentController(IPathHelper pathHelper, IDirectoryWrap directoryWrap, IPathWrap pathWrap)
        {
            _pathHelper = pathHelper;
            _directoryWrap = directoryWrap;
            _pathWrap = pathWrap;
        }

        public async Task<Negotiator> HandleDocumentAddGetAsync(INancyModule module)
        {

            //var userCount = _userRepo.GetUserCountAsync();

            //await Task.WhenAll(userCount);

            DocumentViewModel model = new DocumentViewModel();
            return module.View[Views.Document.Add, model];

        }

        public void HandleDocumentUploadPost(string rootPath, IEnumerable<HttpFile> files)
        {
            var uploadDirectory = _pathHelper.UploadDirectory(rootPath);

            if (!_directoryWrap.Exists(uploadDirectory))
            {
                _directoryWrap.CreateDirectory(uploadDirectory);
            }

            foreach (var file in files)
            {
                var filename = _pathWrap.Combine(uploadDirectory, file.Name);
                using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                {
                    file.Value.CopyTo(fileStream);
                }
            }

        }


    }
}
