using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Test.Apotheca.BLL.TestHelpers;

namespace Test.Apotheca.Integration.Repositories
{
    [TestFixture]
    public class DocumentRepositoryTest : RepositoryIntegrationTestBase
    {
        private IDocumentRepository _repo;
        private UserEntity _user;

        [TestFixtureSetUp]
        public void DocumentRepositoryTest_FixtureSetUp()
        {
            // create a user up front
            IUserRepository userRepo = new UserRepository(this.Connection, this.DbSchema);
            _user = TestEntityHelper.CreateUserWithData();
            userRepo.Create(_user);

        }

        [SetUp]
        public void DocumentRepositoryTest_SetUp()
        {
            _repo = new DocumentRepository(this.Connection, this.DbSchema);

        }

        [Test]
        public void Create()
        {
            DocumentEntity document = TestEntityHelper.CreateDocumentWithData();
            document.CreatedByUserId = _user.Id.Value;
            _repo.Create(document);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetByIdOrDefault(bool includeFileData)
        {
            _repo.GetByIdOrDefault(Guid.NewGuid(), includeFileData);
        }

        [Test]
        public async void GetCountAsync()
        {
            Task<int> count = _repo.GetCountAsync();
            await count;
            Assert.GreaterOrEqual(count.Result, 0);
        }

        [Test]
        public void GetFileContents()
        {
            DocumentEntity document = TestEntityHelper.CreateDocumentWithData();
            document.CreatedByUserId = _user.Id.Value;
            _repo.Create(document);

            byte[] fileContents = _repo.GetFileContents(document.Id.Value);
            Assert.AreEqual(document.FileContents, fileContents);
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
