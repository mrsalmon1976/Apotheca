using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Apotheca.BLL.Resources;
using System.Data;
using Apotheca.BLL.Data;
using System.Configuration;
using Test.Apotheca.Integration.Properties;
using Apotheca.BLL.Database;
using Dapper;

namespace Test.Apotheca.Integration.Repositories
{
    public class RepositoryIntegrationTestBase
    {
        protected IDbContext DbContext { get; private set; }

        [TestFixtureSetUp]
        public void RepositoryIntegrationTestBase_TestFixtureSetUp()
        {
            // set up the database connection
            var connString = Settings.Default.TestDatabaseConnectionString;
            this.DbContext = new DbContext(connString, "test");

            // make sure migrations have run on the test database
            IDbScriptResourceProvider resourceProvider = new DbScriptResourceProvider();
            new DbMigrator().Migrate(this.DbContext, resourceProvider.GetDbMigrationScripts());
        }

        [TestFixtureTearDown]
        public void RepositoryIntegrationTestBase_TestFixtureTearDown()
        {
            using (IDbConnection conn = this.DbContext.GetConnection())
            {
                conn.Execute("DROP FULLTEXT INDEX ON test.[Documents]");
                conn.Execute("DROP FULLTEXT CATALOG [DocumentCatalog]");

                conn.Execute("DROP TABLE test.[AuditLogDetails]");
                conn.Execute("DROP TABLE test.[AuditLogs]");
                conn.Execute("DROP TABLE test.[DocumentVersions]");
                conn.Execute("DROP TABLE test.[Documents]");
                conn.Execute("DROP TABLE test.[Users]");
                conn.Execute("DROP TABLE test.[Categories]");
            }
        }

    }
}
