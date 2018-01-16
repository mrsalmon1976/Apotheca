using DbUp;
using DbUp.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Apotheca.Web.Data
{
    public class DbMigrator
    {
        public static void RunMigrations(string connString)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(connString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .WithTransaction()
                .Build();

            DatabaseUpgradeResult result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                throw new ApplicationException("Database migration error", result.Error);
            }
        }
    }
}
