using Apotheca.Navigation;
using Apotheca.Services;
using Nancy;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;
using Test.Apotheca.TestHelpers;

namespace Test.Apotheca.Services
{
    [TestFixture]
    public class FileUtilityServiceTest
    {
        private IFileUtilityService _fileUtilityService;

        private IPathHelper _pathHelper;
        private IDirectoryWrap _directoryWrap;
        private IPathWrap _pathWrap;
        private IFileWrap _fileWrap;
        private string _rootDir;
        private string _uploadDir;

        [SetUp]
        public void FileUtilityServiceTest_SetUp()
        {
            _pathHelper = Substitute.For<IPathHelper>();
            _directoryWrap = Substitute.For<IDirectoryWrap>();
            _pathWrap = Substitute.For<IPathWrap>();
            _fileWrap = Substitute.For<IFileWrap>();

            _fileUtilityService = new FileUtilityService(_pathHelper, _directoryWrap, _pathWrap, _fileWrap);

            _rootDir = Environment.CurrentDirectory;
            _uploadDir = Path.Combine(_rootDir, "DocumentControllerTest");
            if (Directory.Exists(_uploadDir)) Directory.Delete(_uploadDir, true);
            Directory.CreateDirectory(_uploadDir);

        }

        [Test]
        public void CleanUploadedFiles_FileExistsButNotOldEnough_DoesNotDeleteFile()
        {
            string filePath = Path.Combine(_uploadDir, "test.txt");
            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);
            _directoryWrap.GetFiles(_uploadDir).Returns(Enumerable.Empty<string>());
            CreateTextFile(filePath, "CleanUploadedFiles_FileExistsButNotOldEnough_DoesNotDeleteFile", DateTime.UtcNow);

            int deleted = _fileUtilityService.CleanUploadedFiles(_rootDir);

            Assert.AreEqual(0, deleted);
            _pathHelper.Received(1).UploadDirectory(_rootDir);
            _directoryWrap.Received(1).GetFiles(_uploadDir);
            _fileWrap.DidNotReceive().Delete(Arg.Any<string>());

        }

        [Test]
        public void CleanUploadedFiles_OldFilesExist_DeletesFile()
        {
            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);

            string file1 = Path.Combine(_uploadDir, "1.txt");
            string file2 = Path.Combine(_uploadDir, "2.txt");

            CreateTextFile(file1, "2.txt", DateTime.UtcNow.AddHours(-24).AddSeconds(-1));
            CreateTextFile(file2, "3.txt", DateTime.UtcNow.AddHours(-48));

            _directoryWrap.GetFiles(_uploadDir).Returns(Directory.GetFiles(_uploadDir));

            int deleted = _fileUtilityService.CleanUploadedFiles(_rootDir);

            Assert.AreEqual(2, deleted);
            _pathHelper.Received(1).UploadDirectory(_rootDir);
            _directoryWrap.Received(1).GetFiles(_uploadDir);
            _fileWrap.Received(1).Delete(file1);
            _fileWrap.Received(1).Delete(file2);
        }

        [Test]
        public void CleanUploadedFiles_NewAndOldFilesExist_DeletesOnlyOldFile()
        {
            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);

            string file1 = Path.Combine(_uploadDir, "1.txt");
            string file2 = Path.Combine(_uploadDir, "2.txt");
            string file3 = Path.Combine(_uploadDir, "3.txt");

            CreateTextFile(file1, "1.txt", DateTime.UtcNow.AddHours(-23));
            CreateTextFile(file2, "2.txt", DateTime.UtcNow.AddHours(-24).AddSeconds(-1));
            CreateTextFile(file3, "3.txt", DateTime.UtcNow.AddHours(-48));

            _directoryWrap.GetFiles(_uploadDir).Returns(Directory.GetFiles(_uploadDir));

            int deleted = _fileUtilityService.CleanUploadedFiles(_rootDir);

            Assert.AreEqual(2, deleted);
            _pathHelper.Received(1).UploadDirectory(_rootDir);
            _directoryWrap.Received(1).GetFiles(_uploadDir);
            _fileWrap.DidNotReceive().Delete(file1);
            _fileWrap.Received(1).Delete(file2);
            _fileWrap.Received(1).Delete(file3);
        }

        [Test]
        public void CleanUploadedFiles_NoFiles_Executes()
        {
            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);
            _directoryWrap.GetFiles(_uploadDir).Returns(Directory.GetFiles(_uploadDir));

            int deleted = _fileUtilityService.CleanUploadedFiles(_rootDir);

            Assert.AreEqual(0, deleted);
            _pathHelper.Received(1).UploadDirectory(_rootDir);
            _directoryWrap.Received(1).GetFiles(_uploadDir);
            _fileWrap.DidNotReceive().Delete(Arg.Any<string>());
        }


        [Test]
        public void LoadUploadedFile_OnExecute_LoadsFile()
        {
            // setup 
            string fileName = Path.GetRandomFileName();
            string filePath = Path.Combine(_uploadDir, fileName);

            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);
            _pathWrap.Combine(_uploadDir, fileName).Returns(filePath);
            
            // execute
            _fileUtilityService.LoadUploadedFile(_rootDir, fileName);

            // assert
            _pathHelper.Received(1).UploadDirectory(_rootDir);
            _pathWrap.Received(1).Combine(_uploadDir, fileName);
            _fileWrap.Received(1).ReadAllBytes(filePath);

        }

        [Test]
        public void SaveUploadedFile_DirectoryDoesNotExist_DirectoryCreated()
        {
            // setup
            HttpFile file = new HttpFile("text/plain", "Test.txt", new MemoryStream(TestRandomHelper.GetFileContents(100)), "Test.txt");
            _pathHelper.UploadDirectory(_rootDir).Returns(_uploadDir);
            _directoryWrap.Exists(_uploadDir).Returns(false);
            _pathWrap.Combine(_uploadDir, file.Name).Returns(Path.Combine(Environment.CurrentDirectory, file.Name));

            // execute
            _fileUtilityService.SaveUploadedFile(_rootDir, file);

            // assert
            _directoryWrap.Received(1).Exists(_uploadDir);
            _directoryWrap.Received(1).CreateDirectory(_uploadDir);

        }

        [Test]
        public void SaveUploadedFileFilePosted_FileIsCreated()
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
                HttpFile file = new HttpFile("text/plain", fileName, reader, "Test");
                _fileUtilityService.SaveUploadedFile(_rootDir, file);
            }

            // assert
            _directoryWrap.Received(1).Exists(_uploadDir);
            _directoryWrap.Received(0).CreateDirectory(_uploadDir);
            Assert.True(File.Exists(filePathResult));

            // clean up the files
            File.Delete(filePath);
            File.Delete(filePathResult);

        }

        private void CreateTextFile(string path, string contents, DateTime lastAccessedTimeUtc)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(contents);
                sw.Close();
            }
            FileInfo fi = new FileInfo(path);
            fi.LastAccessTimeUtc = lastAccessedTimeUtc;

        }
    }
}
