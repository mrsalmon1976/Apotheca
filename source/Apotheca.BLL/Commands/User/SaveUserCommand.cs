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
using Apotheca.BLL.Commands.AuditLog;

namespace Apotheca.BLL.Commands.User
{
    public interface ISaveUserCommand : ICommand<Guid>
    {
        UserEntity User { get; set; }

        Guid CurrentUserId { get; set; }

        Guid[] CategoryIds { get; set; }
    }

    public class SaveUserCommand : Command<Guid>, ISaveUserCommand
    {
        private IUnitOfWork _unitOfWork;
        private IUserValidator _userValidator;
        private IRandomKeyGenerator _keyGenerator;
        private IPasswordProvider _passwordProvider;
        private ISaveAuditLogCommand _saveAuditLogCommand;

        public SaveUserCommand(IUnitOfWork unitOfWork, IUserValidator userValidator, IRandomKeyGenerator keyGenerator, IPasswordProvider passwordProvider, ISaveAuditLogCommand saveAuditLogCommand)
        {
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
            _keyGenerator = keyGenerator;
            _passwordProvider = passwordProvider;
            _saveAuditLogCommand = saveAuditLogCommand;
        }

        public UserEntity User { get; set; }

        public Guid CurrentUserId { get; set; }

        public Guid[] CategoryIds { get; set; }
        
        public override Guid Execute()
        {
            if (this.User == null) throw new NullReferenceException("User property cannot be null");
            if (this.CurrentUserId == Guid.Empty) throw new NullReferenceException("CurrentUserId property cannot be null");
            if (_unitOfWork.CurrentTransaction == null) throw new InvalidOperationException("Command must be executed as part of a transaction");

            // set values that are not set by the user: API Key, Salted Password, CreatedOn
            this.User.ApiKey = _keyGenerator.GenerateKey();
            this.User.Salt = _passwordProvider.GenerateSalt();
            this.User.Password = _passwordProvider.HashPassword(this.User.Password, this.User.Salt);

            //validate
            _userValidator.Validate(this.User);

            // set the CreatedOn and insert the new user
            this.User.CreatedOn = DateTime.UtcNow;
            _unitOfWork.UserRepo.Create(this.User);

            // add category ids
            if (this.CategoryIds != null)
            {
                foreach (Guid categoryId in this.CategoryIds)
                {
                    _unitOfWork.UserCategoryAsscRepo.Create(new UserCategoryAsscEntity(this.User.Id, categoryId));
                }
            }
            
            // audit the changes
            _saveAuditLogCommand.AuditLog = AuditLogEntity.Create(typeof(UserEntity).Name, this.User.Id, DbAction.Insert, this.CurrentUserId, this.User);
            _saveAuditLogCommand.Execute();

            return this.User.Id;
        }

    }
}
