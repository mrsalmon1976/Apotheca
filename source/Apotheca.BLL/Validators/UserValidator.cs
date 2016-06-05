using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Validators
{
    public interface IUserValidator
    {
        void Validate(UserEntity user);
    }

    public class UserValidator : IUserValidator
    {
        private IStringValidator _stringValidator;
        private IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository, IStringValidator stringValidator)
        {
            _userRepository = userRepository;
            _stringValidator = stringValidator;
        }

        public void Validate(UserEntity user)
        {
            if (user == null) throw new ArgumentNullException("user");

            List<string> errors = new List<string>();

            if (!_stringValidator.IsValidEmailAddress(user.Email))
            {
                errors.Add("Email address is invalid");
            }

            if (String.IsNullOrWhiteSpace(user.FirstName))
            {
                errors.Add("First name not supplied");
            }
            // we can't do too much with the password as this will be an encrypted value at this stage anyway, 
            // leave the full password validation to the UI level
            if (String.IsNullOrEmpty(user.Password))
            {
                errors.Add("Password not supplied");
            }
            if (String.IsNullOrWhiteSpace(user.ApiKey))
            {
                errors.Add("ApiKey does not exist");
            }
            if (!Roles.AllRoles.Contains(user.Role))
            {
                errors.Add("Role is invalid");
            }

            // all the basic validation is done, if the user is new we need to check that it doesn't exist already
            UserEntity existingUser = _userRepository.GetUserByEmailOrDefault(user.Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                errors.Add(String.Format("A user already exists with email address '{0}'", user.Email));
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

        }
    }
}
