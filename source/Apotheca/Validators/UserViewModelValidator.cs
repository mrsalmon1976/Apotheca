using Apotheca.BLL.Models;
using Apotheca.BLL.Validators;
using Apotheca.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Validators
{
    public interface IUserViewModelValidator
    {
        List<string> Validate(UserViewModel user);
    }

    public class UserViewModelValidator : IUserViewModelValidator
    {
        private IStringValidator _stringValidator;

        public UserViewModelValidator(IStringValidator stringValidator)
        {
            _stringValidator = stringValidator;
        }

        public List<string> Validate(UserViewModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            List<string> errors = new List<string>();

            if (!_stringValidator.IsValidEmailAddress(model.Email))
            {
                errors.Add("Email address is invalid");
            }

            if (String.IsNullOrWhiteSpace(model.FirstName))
            {
                errors.Add("First name not supplied");
            }
            if (String.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters");
            }
            if (model.Password != model.ConfirmPassword)
            {
                errors.Add("Password and confirmation password do not match");
            }
            if (!Roles.AllRoles.Contains(model.Role))
            {
                errors.Add("Role is invalid");
            }

            return errors;
        }
    }
}
