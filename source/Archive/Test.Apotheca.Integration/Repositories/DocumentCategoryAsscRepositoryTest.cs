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
    public class DocumentCategoryAsscRepositoryTest : RepositoryIntegrationTestBase
    {
        private IDocumentCategoryAsscRepository _repo;
        private DocumentEntity _document;
        private CategoryEntity _category;
        private UserEntity _user;

        [TestFixtureSetUp]
        public void DocumentRepositoryTest_FixtureSetUp()
        {
            // create a user, category and document user up front
            IUserRepository userRepo = new UserRepository(this.Connection, this.DbSchema);
            _user = TestEntityHelper.CreateUserWithData();
            userRepo.Create(_user);

            IDocumentRepository documentRepo = new DocumentRepository(this.Connection, this.DbSchema);
            _document = TestEntityHelper.CreateDocumentWithData();
            _document.CreatedByUserId = _user.Id;
            documentRepo.Create(_document);

            ICategoryRepository categoryRepo = new CategoryRepository(this.Connection, this.DbSchema);
            _category = TestEntityHelper.CreateCategoryWithData();
            categoryRepo.Create(_category);

        }

        [SetUp]
        public void DocumentRepositoryTest_SetUp()
        {
            _repo = new DocumentCategoryAsscRepository(this.Connection, this.DbSchema);

        }

        [Test]
        public void Create()
        {
            DocumentCategoryAsscEntity docCatAssc = TestEntityHelper.CreateDocumentCategoryAssc(0, _document.Id, _document.VersionNo, _category.Id);
            _repo.Create(docCatAssc);
        }

        [Test]
        public void GetByDocumentVersion()
        {
            var results = _repo.GetByDocumentVersion(_document.Id, _document.VersionNo);
            Assert.Greater(results.Count(), 0);
        }


    }
}
