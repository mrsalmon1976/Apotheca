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
    public class AuditLogRepositoryTest : RepositoryIntegrationTestBase
    {
        private IAuditLogRepository _repo;
        private UserEntity _user;

        [TestFixtureSetUp]
        public void AuditLogRepositoryTest_FixtureSetUp()
        {
            // create a user up front
            IUserRepository userRepo = new UserRepository(this.Connection, this.DbSchema);
            _user = TestEntityHelper.CreateUserWithData();
            userRepo.Create(_user);

        }

        [SetUp]
        public void AuditLogRepositoryTest_SetUp()
        {
            _repo = new AuditLogRepository(this.Connection, this.DbSchema);

        }

        [Test]
        public void Create()
        {
            AuditLogEntity auditLog = TestEntityHelper.CreateAuditLogWithData();
            auditLog.UserId = _user.Id;
            _repo.Create(auditLog);
            Assert.Greater(auditLog.Id, 0);
        }

        [Test]
        public async void GetLatest()
        {
            Task<IEnumerable<AuditLogDetailModel>> result = _repo.GetLatest(10);
            await result;
            Assert.GreaterOrEqual(result.Result.Count(), 0);
        }



    }
}
