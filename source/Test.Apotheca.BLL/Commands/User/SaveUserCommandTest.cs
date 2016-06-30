﻿using Apotheca.BLL.Commands.User;
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
using Apotheca.BLL.Commands.AuditLog;

namespace Test.Apotheca.BLL.Commands.User
{
    [TestFixture]
    public class SaveUserCommandTest
    {
        private ISaveUserCommand _command;

        private IUnitOfWork _unitOfWork;
        private IDbTransaction _dbTransaction;

        private IUserValidator _userValidator;
        private IUserRepository _userRepo;
        private IUserCategoryAsscRepository _userCategoryAsscRepo;
        private IRandomKeyGenerator _keyGenerator;
        private IPasswordProvider _passwordProvider;
        private ISaveAuditLogCommand _saveAuditLogCommand;

        [SetUp]
        public void CreateUserCommandTest_SetUp()
        {
            _dbTransaction = Substitute.For<IDbTransaction>();

            _userValidator = Substitute.For<IUserValidator>();
            _userRepo = Substitute.For<IUserRepository>();
            _userCategoryAsscRepo = Substitute.For<IUserCategoryAsscRepository>();
            _keyGenerator = Substitute.For<IRandomKeyGenerator>();
            _passwordProvider = Substitute.For<IPasswordProvider>();
            _saveAuditLogCommand = Substitute.For<ISaveAuditLogCommand>();

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _unitOfWork.UserRepo.Returns(_userRepo);
            _unitOfWork.UserCategoryAsscRepo.Returns(_userCategoryAsscRepo);

            _command = new SaveUserCommand(_unitOfWork, _userValidator, _keyGenerator, _passwordProvider, _saveAuditLogCommand);
        }

        [Test]
        public void Execute_UserNull_ThrowsException()
        {
            _command.User = null;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());  
        }

        [Test]
        public void Execute_UserIdEmpty_ThrowsException()
        {
            _command.User = TestEntityHelper.CreateUser();
            _command.CurrentUserId = Guid.Empty;
            Assert.Throws(typeof(NullReferenceException), () => _command.Execute());
        }

        [Test]
        public void Execute_NoTransaction_ThrowsException()
        {
            IDbTransaction transaction = null;
            _unitOfWork.CurrentTransaction.Returns(transaction);
            _command.User = TestEntityHelper.CreateUser();
            _command.CurrentUserId = Guid.NewGuid();
            Assert.Throws(typeof(InvalidOperationException), () => _command.Execute());
        }

        [Test]
        public void Execute_WithUser_Validate()
        {
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = Guid.NewGuid(); });

            _command.User = user;
            _command.CurrentUserId = Guid.NewGuid();
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
            _command.CurrentUserId = Guid.NewGuid();
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
            _command.CurrentUserId = Guid.NewGuid();
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
            _command.CurrentUserId = Guid.NewGuid();
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
            _command.CurrentUserId = Guid.NewGuid();
            Guid result = _command.Execute();

            _userRepo.Received(1).Create(user);
            Assert.AreEqual(id, result);
        }

        [Test]
        public void Execute_NoCategories_DoesNotCreateAnyAssociations()
        {
            Guid id = Guid.NewGuid();
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = id; });

            _command.User = user;
            _command.CurrentUserId = Guid.NewGuid();
            Guid result = _command.Execute();

            _userCategoryAsscRepo.DidNotReceive().Create(Arg.Any<UserCategoryAsscEntity>());
        }

        [Test]
        public void Execute_WithCategories_CreatesAssociations()
        {
            Guid userId = Guid.NewGuid();
            Guid[] categoryIds = { Guid.NewGuid(), Guid.NewGuid() };
            List<Guid> createdCategoryIds = categoryIds.ToList();
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = userId; });
            // set up the repo so that whenever it receives a call, if the user id matches, remove the category id
            _userCategoryAsscRepo.When(x => x.Create(Arg.Any<UserCategoryAsscEntity>())).Do((c) => {
                UserCategoryAsscEntity ucae = c.Args()[0] as UserCategoryAsscEntity;
                if (userId == ucae.UserId)
                {
                    createdCategoryIds.Remove(ucae.CategoryId);
                }
            });

            // execute
            _command.User = user;
            _command.CurrentUserId = Guid.NewGuid();
            _command.CategoryIds = categoryIds;
            Guid result = _command.Execute();

            // the list count should be equal to zero because we should have removed all items in the Do callback above
            Assert.AreEqual(0, createdCategoryIds.Count);
        }


        [Test]
        public void Execute_NewUser_AuditsCreation()
        {
            Guid id = Guid.NewGuid();
            AuditLogEntity auditLog = null;
            UserEntity user = TestEntityHelper.CreateUser();
            _userRepo.When(x => x.Create(user)).Do((c) => { user.Id = id; });
            _saveAuditLogCommand.When(x => x.Execute()).Do((c) => { auditLog = _saveAuditLogCommand.AuditLog; });
            _command.User = user;
            _command.CurrentUserId = Guid.NewGuid();

            // execute
            Guid result = _command.Execute();

            // assert
            _saveAuditLogCommand.Received(1).Execute();

            Assert.IsNotNull(auditLog);
            Assert.AreEqual(typeof(UserEntity).Name, auditLog.Entity);
            Assert.AreEqual(user.Id.ToString(), auditLog.Key);
            Assert.AreEqual(_command.CurrentUserId, auditLog.UserId);
            Assert.AreEqual(AuditLogEntity.Actions.Insert, auditLog.Action);
        }



    }
}
