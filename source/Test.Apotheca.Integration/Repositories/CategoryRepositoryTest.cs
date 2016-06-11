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
using Test.Apotheca.BLL.TestHelpers;

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
            IEnumerable<CategoryEntity> entities = _repo.GetAll();
            Assert.GreaterOrEqual(entities.Count(), 0);
        }

        [Test]
        public void GetAllExtended()
        {
            IEnumerable<CategorySearchResult> entities = _repo.GetAllExtended();
            Assert.GreaterOrEqual(entities.Count(), 0);
        }

        [Test]
        public void Update()
        {
            CategoryEntity cat = TestEntityHelper.CreateCategoryWithData();
            _repo.Create(cat);

            cat.Description = "Changed";
            _repo.Update(cat);
        }


    }
}
