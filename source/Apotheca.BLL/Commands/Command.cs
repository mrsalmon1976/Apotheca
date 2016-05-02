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
        T Execute();
    }

    public abstract class Command<T> : ICommand<T>
    {
        public abstract T Execute();
    }
}
