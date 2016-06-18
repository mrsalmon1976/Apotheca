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
    public class UserCategoryAsscRepositoryTest : RepositoryIntegrationTestBase
    {
        private IUserCategoryAsscRepository _repo;
        private CategoryEntity _category;
        private UserEntity _user;

        [TestFixtureSetUp]
        public void UserCategoryAsscTest_FixtureSetUp()
        {
            // create a user, category and document user up front
            IUserRepository userRepo = new UserRepository(this.Connection, this.DbSchema);
            _user = TestEntityHelper.CreateUserWithData();
            userRepo.Create(_user);

            ICategoryRepository categoryRepo = new CategoryRepository(this.Connection, this.DbSchema);
            _category = TestEntityHelper.CreateCategoryWithData();
            categoryRepo.Create(_category);

        }

        [SetUp]
        public void UserCategoryAsscRepositoryTest_SetUp()
        {
            _repo = new UserCategoryAsscRepository(this.Connection, this.DbSchema);

        }

        [Test]
        public void Create()
        {
            UserCategoryAsscEntity userCatAssc = TestEntityHelper.CreateUserCategoryAssc(0, _user.Id, _category.Id);
            _repo.Create(userCatAssc);
        }

        [Test]
        public void GetByUser()
        {
            var results = _repo.GetByUser(_user.Id);
            Assert.Greater(results.Count(), 0);
        }


    }
}
