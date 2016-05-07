using Apotheca.BLL.Repositories;
using Apotheca.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Apotheca.Modules;
using Nancy;
using Apotheca.ViewModels.Login;
using Apotheca.Web.Results;
using Apotheca.Navigation;
using SystemWrapper.IO;
using System.Reflection;
using System.IO;

namespace Test.Apotheca.Controllers
{
    [TestFixture]
    public class DocumentControllerTest
    {
        private IDocumentController _documentController;
        private IPathHelper _pathHelper;
        private IDirectoryWrap _directoryWrap;
        private IPathWrap _pathWrap;
        private string _rootDir;
        private string _uploadDir;

        [SetUp]
        public void DocumentControllerTest_SetUp()
        {
            _pathHelper = Substitute.For<IPathHelper>();
            _directoryWrap = Substitute.For<IDirectoryWrap>();
            _pathWrap = Substitute.For<IPathWrap>();
            _documentController = new DocumentController(_pathHelper, _directoryWrap, _pathWrap);

            _rootDir = Environment.CurrentDirectory;
            _uploadDir = Path.Combine(_rootDir, "DocumentControllerTest");
            Directory.CreateDirectory(_uploadDir);
        }

        [Test]
        public void HandleDocumentUploadPost_DirectoryDoesNotExist_DirectoryCreated()
        {
            // setup
            HttpFile[] files = new HttpFile[] { };
            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);
            _directoryWrap.Exists(_uploadDir).Returns(false);

            // execute
            _documentController.HandleDocumentUploadPost(_rootDir, files);

            // assert
            _directoryWrap.Received(1).Exists(_uploadDir);
            _directoryWrap.Received(1).CreateDirectory(_uploadDir);
            _pathWrap.DidNotReceive().Combine(Arg.Any<string>(), Arg.Any<string>());

        }

        [Test]
        public void HandleDocumentUploadPost_FilePosted_FileIsCreated()
        {
            // setup - create a physical file for the HttpFile object
            string fileName = "TestResult.txt";
            string filePath = Path.Combine(_uploadDir, "Test.txt");
            string filePathResult = Path.Combine(_uploadDir, fileName);
            File.Delete(filePath);
            File.Delete(filePathResult);
            using (StreamWriter writer = File.CreateText(filePath)) 
            {
                writer.WriteLine("test file");
            }

            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);
            _pathWrap.Combine(_uploadDir, fileName).Returns(filePathResult);
            _directoryWrap.Exists(_uploadDir).Returns(true);


            // execute - we need a stream for the HttpFile
            using (FileStream reader = File.OpenRead(filePath))
            {
                HttpFile[] files = new HttpFile[] { new HttpFile("text/plain", fileName, reader, "Test")  };
                _documentController.HandleDocumentUploadPost(_rootDir, files);

            }

            // assert
            _directoryWrap.Received(1).Exists(_uploadDir);
            _directoryWrap.Received(0).CreateDirectory(_uploadDir);
            Assert.True(File.Exists(filePathResult));

            // clean up the files
            File.Delete(filePath);
            File.Delete(filePathResult);

        }

    }
}
