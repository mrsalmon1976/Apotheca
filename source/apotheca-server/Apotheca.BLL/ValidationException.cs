using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.BLL
{
    public class ValidationException : Exception
    {

        public ValidationException(IEnumerable<string> errors) : base("Validation errors occurred")
        {
            this.Errors = errors;
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
