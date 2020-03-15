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
    public class DocumentVersionRepositoryTest : RepositoryIntegrationTestBase
    {
        private IDocumentVersionRepository _repo;
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
            _repo = new DocumentVersionRepository(this.Connection, this.DbSchema);
        }

        [Test]
        public void Create()
        {
            DocumentEntity document = TestEntityHelper.CreateDocumentWithData();
            document.CreatedByUserId = _user.Id;
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
            DocumentEntity document = TestEntityHelper.CreateDocumentWithData();
            document.CreatedByUserId = _user.Id;
            _repo.Create(document);

            byte[] fileContents = _repo.GetFileContents(document.Id, 1);
            Assert.AreEqual(document.FileContents, fileContents);
        }

        [Test]
        public void GetVersionCount()
        {
            DocumentEntity document = TestEntityHelper.CreateDocumentWithData();
            document.CreatedByUserId = _user.Id;
            _repo.Create(document);

            document.VersionNo = 2;
            _repo.Create(document);

            int count = _repo.GetVersionCount(document.Id);
            Assert.AreEqual(2, count);
        }

    }
}
