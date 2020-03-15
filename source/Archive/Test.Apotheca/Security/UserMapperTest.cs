using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Security;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.Security
{
    [TestFixture]
    public class UserMapperTest
    {
        private IUserMapper _userMapper;

        private IUserRepository _userRepository;

        [SetUp]
        public void UserMapperTest_SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();

            _userMapper = new UserMapper(_userRepository);
        }

        [Test]
        public void GetUserFromIdentifier_NoUser_ReturnsNull()
        {
            Guid userId = Guid.NewGuid();
            _userRepository.GetUserById(Arg.Any<Guid>()).Returns<UserEntity>((ue) => { return null; });

            IUserIdentity userIdentity = _userMapper.GetUserFromIdentifier(userId, new NancyContext());

            Assert.IsNull(userIdentity);
            _userRepository.Received(1).GetUserById(userId);
        }

        [Test]
        public void GetUserFromIdentifier_UserExists_ReturnsUser()
        {
            Guid userId = Guid.NewGuid();
            UserEntity user = new UserEntity();
            user.Email = "test@dsadsad.com";
            user.Id = userId;

            _userRepository.GetUserById(userId).Returns(user);

            IUserIdentity result = _userMapper.GetUserFromIdentifier(userId, new NancyContext());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, user.Email);
            _userRepository.Received(1).GetUserById(userId);
        }

    }
}
