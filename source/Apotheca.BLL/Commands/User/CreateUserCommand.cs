using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Commands.User
{
    public class CreateUserCommand : Command<Guid?>
    {
        public Apotheca.BLL.Models.User User { get; set; }

        public override Guid? Execute()
        {
            // TODO: Add validation

            throw new NotImplementedException();
        }
    }
}
