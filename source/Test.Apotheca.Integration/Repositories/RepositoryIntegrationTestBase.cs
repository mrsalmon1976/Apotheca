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
using System.Data.SqlClient;

namespace Test.Apotheca.Integration.Repositories
{
    public class RepositoryIntegrationTestBase
    {
        protected IDbConnection Connection { get; set; }

        protected string DbSchema { get; set; }

        [TestFixtureSetUp]
        public void RepositoryIntegrationTestBase_TestFixtureSetUp()
        {
            var connString = Settings.Default.TestDatabaseConnectionString;
            this.DbSchema = "test";

            // set up the database connection
            this.Connection = new SqlConnection(connString);
            this.Connection.Open();

            // make sure migrations have run on the test database
            IDbScriptResourceProvider resourceProvider = new DbScriptResourceProvider();
            new DbMigrator().Migrate(this.Connection, this.DbSchema, resourceProvider.GetDbMigrationScripts());
        }

        [TestFixtureTearDown]
        public void RepositoryIntegrationTestBase_TestFixtureTearDown()
        {
            this.Connection.Execute("DROP FULLTEXT INDEX ON test.[Documents]");
            this.Connection.Execute("DROP FULLTEXT CATALOG [DocumentCatalog]");

            this.Connection.Execute("DROP TABLE test.[AuditLogDetails]");
            this.Connection.Execute("DROP TABLE test.[AuditLogs]");
            this.Connection.Execute("DROP TABLE test.[DocumentCategoryAssc]");
            this.Connection.Execute("DROP TABLE test.[DocumentVersions]");
            this.Connection.Execute("DROP TABLE test.[Documents]");
            this.Connection.Execute("DROP TABLE test.[UserCategoryAssc]");
            this.Connection.Execute("DROP TABLE test.[Users]");
            this.Connection.Execute("DROP TABLE test.[Categories]");
            this.Connection.Dispose();
        }

    }
}
