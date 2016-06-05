using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Services;
using Apotheca.Validators;
using Apotheca.ViewModels.Document;
using Apotheca.Web.Results;
using AutoMapper;
using Nancy;
using System;
using System.Collections.Generic;
using System.Web;
using SystemWrapper.IO;

namespace Apotheca.Controllers
{
    public interface IDocumentController
    {
        IControllerResult HandleDocumentAddGet();

        IControllerResult HandleDocumentFormPost(string rootPath, string currentUserName, DocumentViewModel model);

        IControllerResult HandleDocumentDownloadGet(string rootPath, Guid id);

        IControllerResult HandleDocumentSearchGet();

        IControllerResult HandleDocumentSearchPost(DocumentSearchViewModel model);

        void HandleDocumentUploadPost(string rootPath, IEnumerable<HttpFile> files);

        IControllerResult HandleDocumentUpdateGet(string id);

    }

    public class DocumentController : IDocumentController
    {
        private IDocumentViewModelValidator _documentViewModelValidator;
        private IFileUtilityService _fileUtilityService;
        private ISaveDocumentCommand _createDocumentCommand;
        private IUserRepository _userRepository;
        private IDocumentRepository _documentRepository;

        public DocumentController(IDocumentViewModelValidator documentViewModelValidator, IFileUtilityService fileUtilityService, ISaveDocumentCommand createDocumentCommand, IUserRepository userRepository, IDocumentRepository documentRepository)
        {
            _documentViewModelValidator = documentViewModelValidator;
            _fileUtilityService = fileUtilityService;
            _createDocumentCommand = createDocumentCommand;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
        }

        public IControllerResult HandleDocumentAddGet()
        {
            DocumentViewModel model = new DocumentViewModel();
            return new ViewResult(Views.Document.Add, model);
        }

        public IControllerResult HandleDocumentFormPost(string rootPath, string currentUserName, DocumentViewModel model)
        {
            UserEntity user = _userRepository.GetUserByEmail(currentUserName);
            byte[] fileContents = _fileUtilityService.ReadUploadedFile(rootPath, model.UploadedFileName);
            
            // set up the entity
            DocumentEntity document = Mapper.Map<DocumentViewModel, DocumentEntity>(model);
            document.CreatedByUserId = user.Id.Value;
            document.FileContents = fileContents;
            document.MimeType = MimeMapping.GetMimeMapping(document.FileName);

            // do first level validation - if it fails then we need to exit
            List<string> validationErrors = this._documentViewModelValidator.Validate(model);
            if (validationErrors.Count > 0)
            {
                model.ValidationErrors.AddRange(validationErrors);
                return new ViewResult(Views.Document.Add, model);
            }

            // try and execute the command 
            try
            {
                _createDocumentCommand.Document = document;
                _createDocumentCommand.Execute();
            }
            catch (ValidationException vex)
            {
                model.ValidationErrors.AddRange(vex.Errors);
                return new ViewResult(Views.Document.Add, model);
            }

            // if we've got here, we're all good - redirect to the dashboard
            return new RedirectResult(Actions.Dashboard);
        }

        public IControllerResult HandleDocumentDownloadGet(string rootPath, Guid id)
        {
            IFileInfoWrap fileInfo = _fileUtilityService.GetDownloadFileInfo(rootPath, id.ToString());
            FileResult result = new FileResult();

            // TODO: check the current user has access to download the document 

            // get the document, with contents
            DocumentEntity document = _documentRepository.GetByIdOrDefault(id, false);
            if (document == null)
            {
                return new NotFoundResult();
            }

            // save document to disk (Downloads folder) if it does not exist already
            if (fileInfo.Exists)
            {
                result.FileContents = _fileUtilityService.ReadFile(fileInfo.FullName);
            }
            else
            {
                byte[] fileContents = _documentRepository.GetFileContents(id);
                _fileUtilityService.SaveDownloadFile(fileInfo, fileContents);
                result.FileContents = fileContents;
            }
            
            // set up the result
            //result.ApplicationRelativeFilePath = fileInfo.FullName;
            result.ContentType = document.MimeType;
            result.FileName = document.FileName;
            return result;
        }

        public IControllerResult HandleDocumentSearchGet()
        {
            DocumentSearchViewModel model = new DocumentSearchViewModel();
            return new ViewResult(Views.Document.Search, model);
        }

        public IControllerResult HandleDocumentSearchPost(DocumentSearchViewModel model)
        {
            model.Results.AddRange(_documentRepository.Search(model.SearchText, null));
            model.IsResultGridVisible = true;
            return new ViewResult(Views.Document.Search, model);
        }

        public IControllerResult HandleDocumentUpdateGet(string id)
        {
            Guid documentId;
            if (!Guid.TryParse(id, out documentId))
            {
                return new NotFoundResult();
            }

            DocumentEntity document = _documentRepository.GetByIdOrDefault(documentId);
            if (document == null)
            {
                return new NotFoundResult();
            }

            DocumentViewModel model = Mapper.Map<DocumentEntity, DocumentViewModel>(document);
            return new ViewResult(Views.Document.Update, model);
        }

        public void HandleDocumentUploadPost(string rootPath, IEnumerable<HttpFile> files)
        {
            foreach (var file in files)
            {
                _fileUtilityService.SaveUploadedFile(rootPath, file);
            }
        }

    }
}
