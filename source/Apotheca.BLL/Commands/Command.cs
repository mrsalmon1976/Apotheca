using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Commands
{
    public interface ICommand<T>
    {
        IDbConnection DbConnection { get; set; }

        T Execute();
    }

    public abstract class Command<T> : ICommand<T>
    {
        public IDbConnection DbConnection { get; set; }

        public abstract T Execute();
    }
}
