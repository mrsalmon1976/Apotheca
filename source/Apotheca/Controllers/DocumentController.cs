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

namespace Apotheca.Controllers
{
    public interface IDocumentController
    {
        IControllerResult HandleDocumentAddGet();

        IControllerResult HandleDocumentAddPost(string rootPath, string currentUserName, DocumentViewModel model);

        IControllerResult HandleDocumentSearchGet();

        IControllerResult HandleDocumentSearchPost(DocumentSearchViewModel model);

        void HandleDocumentUploadPost(string rootPath, IEnumerable<HttpFile> files);
    }

    public class DocumentController : IDocumentController
    {
        private IDocumentViewModelValidator _documentViewModelValidator;
        private IFileUtilityService _fileUtilityService;
        private ICreateDocumentCommand _createDocumentCommand;
        private IUserRepository _userRepository;
        private IDocumentRepository _documentRepository;

        public DocumentController(IDocumentViewModelValidator documentViewModelValidator, IFileUtilityService fileUtilityService, ICreateDocumentCommand createDocumentCommand, IUserRepository userRepository, IDocumentRepository documentRepository)
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

        public IControllerResult HandleDocumentAddPost(string rootPath, string currentUserName, DocumentViewModel model)
        {
            UserEntity user = _userRepository.GetUserByEmail(currentUserName);
            byte[] fileContents = _fileUtilityService.LoadUploadedFile(rootPath, model.UploadedFileName);
            
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

        public void HandleDocumentUploadPost(string rootPath, IEnumerable<HttpFile> files)
        {
            foreach (var file in files)
            {
                _fileUtilityService.SaveUploadedFile(rootPath, file);
            }
        }

    }
}
