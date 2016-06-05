using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Test.Apotheca.Integration.Repositories
{
    [TestFixture]
    public class DocumentVersionRepositoryTest : RepositoryIntegrationTestBase
    {
        private IDocumentVersionRepository _repo;

        [SetUp]
        public void DocumentRepositoryTest_SetUp()
        {
            _repo = new DocumentVersionRepository(this.DbContext);
        }

        [Test]
        public void Create()
        {
            DocumentEntity document = this.CreateEntity();
            _repo.Create(document);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetByIdOrDefault(bool includeFileData)
        {
            _repo.GetByIdOrDefault(Guid.NewGuid(), 1, includeFileData);
        }

        [Test]
        public void GetFileContents()
        {
            DocumentEntity document = this.CreateEntity();
            _repo.Create(document);

            byte[] fileContents = _repo.GetFileContents(document.Id.Value, 1);
            Assert.AreEqual(document.FileContents, fileContents);
        }

        [Test]
        public void GetVersionCount()
        {
            DocumentEntity document = this.CreateEntity();
            _repo.Create(document);

            document.VersionNo = 2;
            _repo.Create(document);

            int count = _repo.GetVersionCount(document.Id.Value);
            Assert.AreEqual(2, count);
        }

        private DocumentEntity CreateEntity()
        {
            byte[] fileContents = new byte[100];
            new Random().NextBytes(fileContents);


            DocumentEntity document = new DocumentEntity();
            document.Id = Guid.NewGuid();
            document.VersionNo = 1;
            document.CreatedByUserId = Guid.NewGuid();
            document.CreatedOn = DateTime.UtcNow;
            document.Description = "This is a test document!";
            document.Extension = ".txt";
            document.FileContents = fileContents;
            document.FileName = "test.txt";
            document.MimeType = MimeMapping.GetMimeMapping(document.FileName);
            return document;
        }

    }
}
