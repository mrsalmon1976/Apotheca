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
    /// <summary>
    /// Class used for migrating scripts to the database on application startup.  Useful for new versions of the 
    /// application requiring changes to the database.
    /// </summary>
    public class DbMigrator
    {

        public void Migrate(IDbConnection dbConnection, string schema, IDictionary<string, string> scripts)
        {
            // NOTE: you can't use a transaction here due to full index catalogs needing to be created, but 
            // the scripts should be able to be rerun anyway
            foreach (KeyValuePair<string, string> kvp in scripts)
            {
                Console.WriteLine("..." + kvp.Key);
                string sql = kvp.Value.Replace("{SCHEMA}", schema);
                dbConnection.Execute(sql, null, null, 0, null);
            }
        }
    }
}
