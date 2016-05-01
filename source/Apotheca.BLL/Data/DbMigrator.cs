using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Apotheca.BLL.Data;

namespace Apotheca.BLL.Database
{
    public class DbMigrator
    {

        public void Migrate(IDbContext dbContext, string[] scripts)
        {
            IDbConnection conn = dbContext.GetConnection();
            using (IDbTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    foreach (string script in scripts)
                    {
                        string sql = script.Replace("{SCHEMA}", dbContext.Schema);
                        conn.Execute(sql, null, tran, 0, null);
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
