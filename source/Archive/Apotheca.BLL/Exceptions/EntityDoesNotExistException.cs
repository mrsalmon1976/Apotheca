using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Exceptions
{
    public class EntityDoesNotExistException : Exception
    {
        public EntityDoesNotExistException(string criteria, object val)
            : base(String.Format("Entity does not exist using criteria [{0}], value [{1}]", criteria, val))
        {
        }
    }
}
