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
    public class DocumentRepositoryTest : RepositoryIntegrationTestBase
    {
        private IDocumentRepository _repo;

        [SetUp]
        public void DocumentRepositoryTest_SetUp()
        {
            _repo = new DocumentRepository(this.DbContext);
        }

        [Test]
        public void Create()
        {
            byte[] fileContents = new byte[100];
            new Random().NextBytes(fileContents);


            DocumentEntity document = new DocumentEntity();
            document.CreatedByUserId = Guid.NewGuid();
            document.CreatedOn = DateTime.UtcNow;
            document.Description = "This is a test document!";
            document.Extension = ".txt";
            document.FileContents = fileContents;
            document.FileName = "test.txt";
            document.MimeType = MimeMapping.GetMimeMapping(document.FileName);

            _repo.Create(document);
        }

        [Test]
        public async void GetCountAsync()
        {
            Task<int> count = _repo.GetCountAsync();
            await count;
            Assert.GreaterOrEqual(count.Result, 0);
        }

        [TestCase("test", "")]
        public void Search(string text, string categories)
        {
            IEnumerable<int> cats = categories.Split(',').Select(x => Convert.ToInt32(x));
            IEnumerable<DocumentSearchResult> results = _repo.Search(text, cats);
            Assert.GreaterOrEqual(results.Count(), 0);
        }

    }
}
