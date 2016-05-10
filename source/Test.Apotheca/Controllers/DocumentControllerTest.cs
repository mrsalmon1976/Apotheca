using Apotheca.BLL.Commands.Document;
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

        [SetUp]
        public void DocumentControllerTest_SetUp()
        {
            _fileUtilityService = Substitute.For<IFileUtilityService>();
            _documentViewModelValidator = Substitute.For<IDocumentViewModelValidator>();
            _createDocumentCommand = Substitute.For<ICreateDocumentCommand>();
            _userRepository = Substitute.For<IUserRepository>();

            Mapper.Reset();
            Mapper.Initialize((cfg) =>
            {
                cfg.CreateMap<DocumentViewModel, DocumentEntity>();
            });
            _documentController = new DocumentController(_documentViewModelValidator, _fileUtilityService, _createDocumentCommand, _userRepository);

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
            Assert.Fail();
        }

        [Test]
        public void HandleDocumentAddPost_FailsDataValidation_DisplaysView()
        {
            Assert.Fail();
        }

        [Test]
        public void HandleDocumentAddPost_OnSuccess_RedirectsToDashboard()
        {
            // setup 
            string rootPath = Environment.CurrentDirectory;
            string currentUserName = Path.GetRandomFileName() + "@test.com";
            DocumentViewModel model = TestViewModelHelper.CreateDocumentViewModelWithData();
            byte[] fileContents = TestRandomHelper.GetFileContents(100);

            _fileUtilityService.LoadUploadedFile(rootPath, model.UploadedFileName).Returns(fileContents);
            _documentViewModelValidator.Validate(model).Returns(new System.Collections.Generic.List<string>());

            // execute
            RedirectResult result = _documentController.HandleDocumentAddPost(rootPath, currentUserName, model) as RedirectResult;
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Actions.Dashboard, result.Location);
            _createDocumentCommand.Received(1).Execute();
        }

        #endregion

    }
}
