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
        Task<User> CreateUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IUserValidator _userValidator;
        private readonly IPasswordProvider _passwordProvider;

        public UserService(IUserRepository userRepo, IStoreRepository storeRepo, IUserValidator userValidator, IPasswordProvider passwordProvider)
        {
            this._userRepo = userRepo;
            this._storeRepo = storeRepo;
            this._userValidator = userValidator;
            this._passwordProvider = passwordProvider;
        }

        public async Task<User> CreateUser(User user)
        {
            _userValidator.Validate(user);

            // make sure the user doesn't already exist
            User userCheck = await _userRepo.GetByEmail(user.Email);
            if (userCheck != null)
            {
                throw new ValidationException("A user with this email address already exists");
            }

            // create a copy of the user (so we don't affect the supplied object), but hash the password
            byte[] salt = _passwordProvider.GenerateSalt();
            User u = new User()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Salt = salt,
                Password = _passwordProvider.HashPassword(user.Password, salt),
            };

            // create a document store for the user
            Store store = new Store();
            store.Name = Constants.MyStoreName;
            store.Subscribers.Add(new StoreSubscriber(u.Id, StoreRole.Admin));

            // add a reference to the store back to the user for retrieval
            u.Stores.Add(store.Id);

            _userRepo.Insert(u);
            _storeRepo.Insert(store);

            return u;
        }
    }
}
