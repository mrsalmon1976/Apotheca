using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Validators
{
    public interface IStringValidator
    {
        bool IsValidEmailAddress(string email);
    }

    public class StringValidator : IStringValidator
    {
        public bool IsValidEmailAddress(string email)
        {
            if (String.IsNullOrEmpty(email)) return false;
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
