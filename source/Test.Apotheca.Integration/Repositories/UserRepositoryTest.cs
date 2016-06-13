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
    public class UserRepositoryTest : RepositoryIntegrationTestBase
    {
        private IUserRepository _repo;

        [SetUp]
        public void UserRepositoryTest_SetUp()
        {
            _repo = new UserRepository(this.Connection, this.DbSchema);
        }

        [Test]
        public void Create()
        {
            byte[] password = new byte[10];
            new Random().NextBytes(password);


            UserEntity user = new UserEntity();
            user.ApiKey = new RandomKeyGenerator().GenerateKey();
            user.CreatedOn = DateTime.UtcNow;
            user.Email = "test@test.com";
            user.FirstName = "Joe";
            user.Password = Convert.ToBase64String(password);
            user.Role = Roles.User;
            user.Salt = Guid.NewGuid().ToString();
            user.Surname = "Soap";
            _repo.Create(user);
        }

        [Test]
        public void GetAllExtended()
        {
            _repo.GetAllExtended();
        }

        [Test]
        public void GetUserByEmail()
        {
            try
            {
                _repo.GetUserByEmail("GetUserByEmail@test.com");
            }
            catch (EntityDoesNotExistException) 
            {
                // we can ignore this for test purposes
            }
        }

        [Test]
        public void GetUserByEmailOrDefault()
        {
            _repo.GetUserByEmailOrDefault("test@test.com");
        }

        [Test]
        public void GetUserById()
        {
            try
            {
                _repo.GetUserByIdOrDefault(Guid.NewGuid());
            }
            catch (EntityDoesNotExistException)
            {
                // we can ignore this for test purposes
            }
        }

        [Test]
        public void GetUserByIdOrDefault()
        {
            _repo.GetUserByIdOrDefault(Guid.NewGuid());
        }

        [Test]
        public async void GetUserCountAsync()
        {
            Task<int> count = _repo.GetUserCountAsync();
            await count;
            Assert.GreaterOrEqual(count.Result, 0);
        }

        [Test]
        public void UsersExist()
        {
            _repo.UsersExist();
        }

    }
}
