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

        public ValidationException(string error) : base("Validation errors occurred")
        {
            this.Errors = new string[] { error };
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
