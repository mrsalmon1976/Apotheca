using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Apotheca.BLL.Database
{
    public interface IDbMigrator
    {
        void Migrate(string schemaName, string[] scripts);
    }

    public class DbMigrator : IDbMigrator
    {
        private IDbConnection _conn;

        public DbMigrator(IDbConnection connection)
        {
            _conn = connection;
        }

        public void Migrate(string schemaName, string[] scripts)
        {
            using (IDbTransaction tran = _conn.BeginTransaction())
            {
                try
                {
                    foreach (string script in scripts)
                    {
                        string sql = script.Replace("{SCHEMA}", schemaName);
                        _conn.Execute(sql, null, tran, 0, null);
                    }
                    tran.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
