using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Security;
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
    public class CategoryRepositoryTest : RepositoryIntegrationTestBase
    {
        private ICategoryRepository _repo;

        [SetUp]
        public void CategoryRepositoryTest_SetUp()
        {
            _repo = new CategoryRepository(this.Connection, this.DbSchema);
        }

        [Test]
        public void Create()
        {
            CategoryEntity category = new CategoryEntity();
            category.Name = "test";
            category.Description = "test description";
            category.CreatedOn = DateTime.UtcNow;
            _repo.Create(category);
        }

        [Test]
        public void GetAll()
        {
            IEnumerable<CategorySearchResult> entities = _repo.GetAll();
            Assert.GreaterOrEqual(entities.Count(), 0);
        }

    }
}
