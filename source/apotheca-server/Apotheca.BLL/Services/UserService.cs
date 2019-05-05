using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Security;
using Apotheca.BLL.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Services
{
    public interface IUserService
    {
        User CreateUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserValidator _userValidator;
        private readonly IPasswordProvider _passwordProvider;

        public UserService(IUserRepository userRepo, IUserValidator userValidator, IPasswordProvider passwordProvider)
        {
            this._userRepo = userRepo;
            this._userValidator = userValidator;
            this._passwordProvider = passwordProvider;
        }

        public User CreateUser(User user)
        {
            _userValidator.Validate(user);

            // make sure the user doesn't already exist
            User userCheck = Task.Run<User>(() => _userRepo.GetUserByEmail(user.Email)).Result;
            if (userCheck != null)
            {
                throw new ValidationException("A user with this email address already exists");
            }

            // create a copy of the user (so we don't affect the supplied object), but hash the password
            byte[] salt = _passwordProvider.GenerateSalt();
            User u = new User()
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Salt = salt,
                Password = _passwordProvider.HashPassword(user.Password, salt),
            };
            _userRepo.Insert(u);

            return u;
        }
    }
}
