using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using Apotheca.Navigation;
using Apotheca.Services;
using Apotheca.Validators;
using Apotheca.ViewModels.Document;
using Apotheca.Web.Results;
using AutoMapper;
using Nancy;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using SystemWrapper.IO;
using Test.Apotheca.BLL.TestHelpers;
using Test.Apotheca.TestHelpers;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class DocumentControllerTest
    {
        private IDocumentController _documentController;
        private IFileUtilityService _fileUtilityService;
        private IDocumentViewModelValidator _documentViewModelValidator;
        private ICreateDocumentCommand _createDocumentCommand;
        private IUserRepository _userRepository;
        private IDocumentRepository _documentRepository;

        [SetUp]
        public void DocumentControllerTest_SetUp()
        {
            _fileUtilityService = Substitute.For<IFileUtilityService>();
            _documentViewModelValidator = Substitute.For<IDocumentViewModelValidator>();
            _createDocumentCommand = Substitute.For<ICreateDocumentCommand>();
            _userRepository = Substitute.For<IUserRepository>();
            _documentRepository = Substitute.For<IDocumentRepository>();

            Mapper.Reset();
            Mapper.Initialize((cfg) =>
            {
                cfg.CreateMap<DocumentViewModel, DocumentEntity>();
            });
            _documentController = new DocumentController(_documentViewModelValidator, _fileUtilityService, _createDocumentCommand, _userRepository, _documentRepository);

        }

        #region HandleDocumentUploadPost Tests

        [TestCase("Test.txt")]
        [TestCase("Test.txt", "Test2.txt")]
        [TestCase("Test.txt", "Test2.txt", "Test3.txt")]
        public void HandleDocumentUploadPost_FilesPosted_Saved(params string[] files)
        {
            // set up
            int fileCount = files.Length;
            string rootPath = Environment.CurrentDirectory;
            HttpFile[] httpFiles = new HttpFile[files.Length];
            for (int i=0; i<files.Length; i++)
            {
                httpFiles[i] = new HttpFile("text/plain", files[i], new MemoryStream(TestRandomHelper.GetFileContents(100)), files[i]);
            }

            // execute 
            _documentController.HandleDocumentUploadPost(rootPath, httpFiles);

            // assert
            _fileUtilityService.Received(files.Length).SaveUploadedFile(rootPath, Arg.Any<HttpFile>());
        }

        #endregion

        #region HandleDocumentAddPost Tests

        [Test]
        public void HandleDocumentAddPost_FailsModelValidation_DisplaysView()
        {
            // setup 
            string rootPath = Environment.CurrentDirectory;
            string currentUserName = Path.GetRandomFileName() + "@test.com";
            DocumentViewModel model = TestViewModelHelper.CreateDocumentViewModelWithData();
            byte[] fileContents = TestRandomHelper.GetFileContents(100);

            UserEntity user = new UserEntity() { Email = currentUserName, Id = Guid.NewGuid() };
            _userRepository.GetUserByEmail(currentUserName).Returns(user);

            _fileUtilityService.ReadUploadedFile(rootPath, model.UploadedFileName).Returns(fileContents);
            _documentViewModelValidator.Validate(model).Returns(new System.Collections.Generic.List<string>() { "error" });

            // execute
            ViewResult result = _documentController.HandleDocumentAddPost(rootPath, currentUserName, model) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Document.Add, result.ViewName);
            _createDocumentCommand.DidNotReceive().Execute();
        }

        [Test]
        public void HandleDocumentAddPost_FailsDataValidation_DisplaysView()
        {
            // setup 
            string rootPath = Environment.CurrentDirectory;
            string currentUserName = Path.GetRandomFileName() + "@test.com";
            DocumentViewModel model = TestViewModelHelper.CreateDocumentViewModelWithData();
            byte[] fileContents = TestRandomHelper.GetFileContents(100);

            UserEntity user = new UserEntity() { Email = currentUserName, Id = Guid.NewGuid() };
            _userRepository.GetUserByEmail(currentUserName).Returns(user);

            _fileUtilityService.ReadUploadedFile(rootPath, model.UploadedFileName).Returns(fileContents);
            _documentViewModelValidator.Validate(model).Returns(new System.Collections.Generic.List<string>());

            _createDocumentCommand.When(x => x.Execute()).Do((args) => { throw new ValidationException("error"); });

            // execute
            ViewResult result = _documentController.HandleDocumentAddPost(rootPath, currentUserName, model) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Views.Document.Add, result.ViewName);
            _createDocumentCommand.Received(1).Execute();
        }

        [Test]
        public void HandleDocumentAddPost_OnSuccess_RedirectsToDashboard()
        {
            // setup 
            string rootPath = Environment.CurrentDirectory;
            string currentUserName = Path.GetRandomFileName() + "@test.com";
            DocumentViewModel model = TestViewModelHelper.CreateDocumentViewModelWithData();
            byte[] fileContents = TestRandomHelper.GetFileContents(100);

            UserEntity user = new UserEntity() { Email = currentUserName, Id = Guid.NewGuid() };
            _userRepository.GetUserByEmail(currentUserName).Returns(user);

            _fileUtilityService.ReadUploadedFile(rootPath, model.UploadedFileName).Returns(fileContents);
            _documentViewModelValidator.Validate(model).Returns(new System.Collections.Generic.List<string>());

            // execute
            RedirectResult result = _documentController.HandleDocumentAddPost(rootPath, currentUserName, model) as RedirectResult;
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Actions.Dashboard, result.Location);
            _createDocumentCommand.Received(1).Execute();
            _userRepository.Received(1).GetUserByEmail(currentUserName);
        }

        #endregion

        #region HandleDocumentDownloadGet Tests

        [Test]
        public void HandleDocumentDownloadGet_NoDocument_Returns404()
        {
            string rootPath = Environment.CurrentDirectory;
            Guid documentId = Guid.NewGuid();
            string fileName = documentId.ToString();
            IFileInfoWrap fileInfo = Substitute.For<IFileInfoWrap>();
            DocumentEntity documentEntity = null;

            _fileUtilityService.GetDownloadFileInfo(rootPath, fileName).Returns(fileInfo);
            _documentRepository.GetByIdOrDefault(documentId, false).Returns(documentEntity);

            // execute
            NotFoundResult result = _documentController.HandleDocumentDownloadGet(rootPath, documentId) as NotFoundResult;

            Assert.IsNotNull(result);
            _fileUtilityService.Received(1).GetDownloadFileInfo(rootPath, fileName);
            _documentRepository.Received(1).GetByIdOrDefault(documentId, false);

        }

        [Test]
        public void HandleDocumentDownloadGet_DocumentFound_SavesToDisk()
        {
            string rootPath = Environment.CurrentDirectory;
            Guid documentId = Guid.NewGuid();
            string fileName = documentId.ToString();
            IFileInfoWrap fileInfo = Substitute.For<IFileInfoWrap>();
            fileInfo.Exists.Returns(false);
            DocumentEntity documentEntity = TestEntityHelper.CreateDocumentWithData();
            byte[] fileContents = documentEntity.FileContents;

            _fileUtilityService.GetDownloadFileInfo(rootPath, fileName).Returns(fileInfo);
            _documentRepository.GetByIdOrDefault(documentId, false).Returns(documentEntity);
            _documentRepository.GetFileContents(documentId).Returns(fileContents);

            // execute
            _documentController.HandleDocumentDownloadGet(rootPath, documentId);

            _documentRepository.Received(1).GetFileContents(documentId);
            _fileUtilityService.Received(1).SaveDownloadFile(fileInfo, fileContents);
        }

        [Test]
        public void HandleDocumentDownloadGet_DocumentFound_ReturnsFileResult()
        {
            string rootPath = Environment.CurrentDirectory;
            Guid documentId = Guid.NewGuid();
            string fileName = documentId.ToString();
            string filePath = Path.Combine(rootPath, fileName);

            IFileInfoWrap fileInfo = Substitute.For<IFileInfoWrap>();
            fileInfo.Exists.Returns(true);
            fileInfo.FullName.Returns(filePath);

            DocumentEntity documentEntity = TestEntityHelper.CreateDocumentWithData();
            byte[] fileContents = documentEntity.FileContents;

            _fileUtilityService.GetDownloadFileInfo(rootPath, fileName).Returns(fileInfo);
            _documentRepository.GetByIdOrDefault(documentId, false).Returns(documentEntity);

            // execute
            FileResult result = _documentController.HandleDocumentDownloadGet(rootPath, documentId) as FileResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(filePath, result.ApplicationRelativeFilePath);
            Assert.AreEqual(documentEntity.MimeType, result.ContentType);
        }

        #endregion

    }
}
