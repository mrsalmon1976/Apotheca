using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Commands
{
    public interface ICommandFactory
    {
        TCommand CreateCommand<TCommand, TReturnType>() where TCommand : ICommand<TReturnType>;
    }
    
    public class CommandFactory : ICommandFactory
    {
        private IDbConnection _dbConnection;

        public CommandFactory(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public TCommand CreateCommand<TCommand, TReturnType>() where TCommand : ICommand<TReturnType>
        {
            TCommand cmd = Activator.CreateInstance<TCommand>();
            //cmd.DbConnection = _dbConnection;
            return cmd;
        }
    }
}
