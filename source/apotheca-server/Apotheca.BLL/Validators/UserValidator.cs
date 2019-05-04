using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL.Validators
{
    public interface IUserValidator
    {
        void Validate(User user);
    }

    public class UserValidator : IUserValidator
    {
        private readonly IEmailValidator _emailValidator;

        public UserValidator(IEmailValidator emailValidator)
        {
            this._emailValidator = emailValidator;
        }

        public void Validate(User user)
        {
            List<string> errors = new List<string>();

            if (user == null) throw new NullReferenceException("No user object supplied");

            if (String.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add("Email address is required");
            }
            else
            {
                if (!_emailValidator.IsValidEmail(user.Email))
                {
                    errors.Add($"'{user.Email}' is not a valid email address");
                }
            }

            if (String.IsNullOrWhiteSpace(user.FirstName))
            {
                errors.Add("First name is required");
            }

            if (String.IsNullOrWhiteSpace(user.LastName))
            {
                errors.Add("Last name is required");
            }

            if (String.IsNullOrEmpty(user.Password))
            {
                errors.Add("Password is required");
            }
            else if (user.Password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
