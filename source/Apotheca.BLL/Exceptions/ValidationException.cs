using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string msg, string error) : this(msg, new string[] { error })
        {
            
        }

        public ValidationException(string error) : this("Validation has failed", error)
        {
        }

        public ValidationException(IEnumerable<string> errors) : this("Validation has failed", errors)
        {
        }

        public ValidationException(string msg, IEnumerable<string> errors) : base(msg)
        {
            this.Errors = new List<string>();
            this.Errors.AddRange(errors);
        }

        public List<string> Errors { get; private set; }
    }
}
