using Apotheca.BLL.Validators;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apotheca.BLL.Security;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Data;

namespace Apotheca.BLL.Commands.User
{
    public interface ICreateUserCommand : ICommand<Guid>
    {
        UserEntity User { get; set; }
    }

    public class CreateUserCommand : Command<Guid>, ICreateUserCommand
    {
        private IDbContext _dbContext;
        private IUserValidator _userValidator;
        private IUserRepository _userRepo;
        private IRandomKeyGenerator _keyGenerator;
        private IPasswordProvider _passwordProvider;

        public CreateUserCommand(IDbContext dbContext, IUserValidator userValidator, IUserRepository userRepo, IRandomKeyGenerator keyGenerator, IPasswordProvider passwordProvider)
        {
            _dbContext = dbContext;
            _userValidator = userValidator;
            _userRepo = userRepo;
            _keyGenerator = keyGenerator;
            _passwordProvider = passwordProvider;
        }

        public UserEntity User { get; set; }
        
        public override Guid Execute()
        {
            if (this.User == null) throw new NullReferenceException("User property cannot be null");

            // set values that are not set by the user: API Key, Salted Password, CreatedOn
            this.User.ApiKey = _keyGenerator.GenerateKey();
            this.User.Salt = _passwordProvider.GenerateSalt();
            this.User.Password = _passwordProvider.HashPassword(this.User.Password, this.User.Salt);

            //validate
            _userValidator.Validate(this.User);

            // start a transaction as now we're hitting the database
            IDbTransaction txn = this._dbContext.BeginTransaction();
            try
            {
                // set the CreatedOn and insert the new user
                this.User.CreatedOn = DateTime.UtcNow;
                _userRepo.Create(this.User);

                txn.Commit();
            }
            catch (Exception)
            {
                if (txn != null) txn.Rollback();
                throw;
            }
            return this.User.Id.Value;
        }

    }
}
