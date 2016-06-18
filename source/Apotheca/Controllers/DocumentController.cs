using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Data;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Services;
using Apotheca.Validators;
using Apotheca.ViewModels;
using Apotheca.ViewModels.Document;
using Apotheca.Web.Results;
using AutoMapper;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IUnitOfWork _unitOfWork;
        private IDocumentViewModelValidator _documentViewModelValidator;
        private IFileUtilityService _fileUtilityService;
        private ISaveDocumentCommand _createDocumentCommand;

        public DocumentController(IUnitOfWork unitOfWork, IDocumentViewModelValidator documentViewModelValidator, IFileUtilityService fileUtilityService, ISaveDocumentCommand createDocumentCommand)
        {
            _unitOfWork = unitOfWork;
            _documentViewModelValidator = documentViewModelValidator;
            _fileUtilityService = fileUtilityService;
            _createDocumentCommand = createDocumentCommand;
        }

        public IControllerResult HandleDocumentAddGet()
        {
            DocumentViewModel model = new DocumentViewModel();
            var categories = this._unitOfWork.CategoryRepo.GetAll();
            var options = categories.Select(x => new MultiSelectItem(x.Id.ToString(), x.Name, false));
            model.CategoryOptions.AddRange(options);
            return new ViewResult(Views.Document.Add, model);
        }

        public IControllerResult HandleDocumentFormPost(string rootPath, string currentUserName, DocumentViewModel model)
        {
            UserEntity user = _unitOfWork.UserRepo.GetUserByEmail(currentUserName);
            byte[] fileContents = _fileUtilityService.ReadUploadedFile(rootPath, model.UploadedFileName);
            
            // set up the entity
            DocumentEntity document = Mapper.Map<DocumentViewModel, DocumentEntity>(model);
            document.CreatedByUserId = user.Id;
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
                _unitOfWork.BeginTransaction();
                _createDocumentCommand.Document = document;
                _createDocumentCommand.Categories = model.CategoryIds;
                _createDocumentCommand.Execute();
                _unitOfWork.Commit();
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
            DocumentEntity document = _unitOfWork.DocumentRepo.GetByIdOrDefault(id, false);
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
                byte[] fileContents = _unitOfWork.DocumentRepo.GetFileContents(id);
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
            model.Results.AddRange(_unitOfWork.DocumentRepo.Search(model.SearchText, null));
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

            DocumentEntity document = _unitOfWork.DocumentRepo.GetByIdOrDefault(documentId);
            if (document == null)
            {
                return new NotFoundResult();
            }

            IEnumerable<Guid> documentCategoryIds = _unitOfWork.DocumentCategoryAsscRepo.GetByDocumentVersion(document.Id, document.VersionNo).Select(x => x.CategoryId);

            var categories = this._unitOfWork.CategoryRepo.GetAll();
            var options = categories.Select(x => new MultiSelectItem(x.Id.ToString(), x.Name, documentCategoryIds.Contains(x.Id)));

            DocumentViewModel model = Mapper.Map<DocumentEntity, DocumentViewModel>(document);
            model.CategoryOptions.AddRange(options);
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
