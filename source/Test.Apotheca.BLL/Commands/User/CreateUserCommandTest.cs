using Apotheca.BLL.Commands.User;
using Apotheca.BLL.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Apotheca.BLL.Models;
using Test.Apotheca.BLL.TestHelpers;
using Apotheca.BLL.Security;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Data;
using System.Data;

namespace Test.Apotheca.BLL.Commands.User
{
    [TestFixture]
    public class CreateUserCommandTest
    {
        private ICreateUserCommand _command;

        private IDbContext _dbContext;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        private IUserValidator _userValidator;
        private IUserRepository _userRepo;
        private IRandomKeyGenerator _keyGenerator;
        private IPasswordProvider _passwordProvider;

        [SetUp]
        public void CreateUserCommandTest_SetUp()
        {
            _dbContext = Substitute.For<IDbContext>();
            _dbConnection = Substitute.For<IDbConnection>();
            _dbContext.GetConnection().Returns(_dbConnection);
            _dbTransaction = Substitute.For<IDbTransaction>();
            _dbContext.BeginTransaction().Returns(_dbTransaction);

            _userValidator = Substitute.For<IUserValidator>();
            _userRepo = Substitute.For<IUserRepository>();
            _keyGenerator = Substitute.For<IRandomKeyGenerator>();
            _passwordProvider = Substitute.For<IPasswordProvider>();
            
            _command = new CreateUserCommand(_dbContext, _userValidator, _userRepo, _keyGenerator, _passwordProvider);
        }

        [Test]
        public void Execute_UserNull_ThrowsException()
        {
            _command.User = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_WithUser_Validate()
        {
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = Guid.NewGuid(); });

            _command.User = user;
            _command.Execute();

            _userValidator.Received(1).Validate(_command.User);
        }

        [Test]
        public void Execute_WithUser_SetsApiKey()
        {
            string key = Guid.NewGuid().ToString();
            UserEntity user = TestEntityHelper.CreateUser();
            _keyGenerator.GenerateKey().Returns(key);
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = Guid.NewGuid(); });

            _command.User = user;
            _command.Execute();

            _keyGenerator.Received(1).GenerateKey();
            Assert.AreEqual(key, user.ApiKey);
        }

        [Test]
        public void Execute_WithUser_SetsPasswordAndSalt()
        {
            string salt = Guid.NewGuid().ToString();
            string textPassword = Guid.NewGuid().ToString();
            string hashedPassword = Guid.NewGuid().ToString();
            
            UserEntity user = TestEntityHelper.CreateUser();
            user.Password = textPassword;
            
            _passwordProvider.GenerateSalt().Returns(salt);
            _passwordProvider.HashPassword(textPassword, salt).Returns(hashedPassword);

            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = Guid.NewGuid(); });

            // execute
            _command.User = user;
            _command.Execute();

            _passwordProvider.Received(1).GenerateSalt();
            _passwordProvider.Received(1).HashPassword(textPassword, salt);

            Assert.AreEqual(salt, user.Salt);
            Assert.AreEqual(hashedPassword, user.Password);
        }

        [Test]
        public void Execute_WithUser_SetsCreatedOn()
        {
            UserEntity user = TestEntityHelper.CreateUser();
            user.CreatedOn = null;
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = Guid.NewGuid(); });

            DateTime dtBefore = DateTime.UtcNow;
            _command.User = user;
            _command.Execute();
            DateTime dtAfter = DateTime.UtcNow;

            _keyGenerator.Received(1).GenerateKey();
            Assert.IsNotNull(user.CreatedOn);
            Assert.LessOrEqual(dtBefore, user.CreatedOn);
            Assert.GreaterOrEqual(dtAfter, user.CreatedOn);
        }

        [Test]
        public void Execute_WithUser_SavesAndReturnsId()
        {
            Guid id = Guid.NewGuid();
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = id; });
            
            _command.User = user;
            Guid result = _command.Execute();

            _userRepo.Received(1).Create(user);
            Assert.AreEqual(id, result);
        }

        [Test]
        public void Execute_WithUser_UsesTransaction()
        {
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = Guid.NewGuid(); });

            _command.User = user;
            Guid result = _command.Execute();

            _dbContext.Received(1).BeginTransaction();
            _dbTransaction.Received(1).Commit();

        }

    }
}
