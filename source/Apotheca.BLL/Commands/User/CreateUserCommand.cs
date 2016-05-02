using Apotheca.BLL.Validators;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Commands.User
{
    public interface ICreateUserCommand : ICommand<Guid>
    {
        Apotheca.BLL.Models.UserEntity User { get; set; }
    }

    public class CreateUserCommand : Command<Guid>, ICreateUserCommand
    {
        private IUserValidator _userValidator;

        public CreateUserCommand(IUserValidator stringValidator)
        {
            _userValidator = stringValidator;
        }

        public Apotheca.BLL.Models.UserEntity User { get; set; }
        
        public override Guid Execute()
        {
            // validate
            if (this.User == null) throw new NullReferenceException("User property cannot be null");
            _userValidator.Validate(this.User);

            // TODO: Insert the new user

            // TODO: return the Guid of the new user
            return Guid.NewGuid();
        }

    }
}
