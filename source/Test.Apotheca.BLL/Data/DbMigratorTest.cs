using Apotheca.BLL.Database;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Apotheca.BLL.Data;

namespace Test.Apotheca.BLL.Data
{
    [TestFixture]
    public class DbMigratorTest
    {
        private DbMigrator _dbMigrator;
        private IDbConnection _dbConnection;

        public DbMigratorTest()
        {
        }

        [SetUp]
        public void DbMigratorTest_SetUp()
        {
            _dbConnection = Substitute.For<IDbConnection>();
            _dbMigrator = new DbMigrator();
        }

        /// <summary>
        /// Ensures that all scripts are executed.
        /// </summary>
        [Test]
        public void Migrate_WhenExecuted_ExecutesAllScripts()
        {
            string script1 = Guid.NewGuid().ToString();
            string script2 = Guid.NewGuid().ToString();
            string script3 = Guid.NewGuid().ToString();
            string[] scripts = { script1, script2, script3 };
            const string schema = "dbo";
            //_dbConnection.Schema.Returns(schema);

            _dbMigrator.Migrate(_dbConnection, schema, scripts);

            _dbConnection.Received(3).Execute(Arg.Any<string>());
            _dbConnection.Received().Execute(script1, null, Arg.Any<IDbTransaction>(), 0, null);
            _dbConnection.Received().Execute(script2, null, Arg.Any<IDbTransaction>(), 0, null);
            _dbConnection.Received().Execute(script3, null, Arg.Any<IDbTransaction>(), 0, null);
        }

        /// <summary>
        /// Ensures that all scripts are executed.
        /// </summary>
        [Test]
        public void Migrate_WhenExecuted_ReplacesSchema()
        {
            string s1 = "SELECT * FROM [{SCHEMA}].MyTable";
            string[] scripts = { s1 };
            string schema = new Random().Next(1000, 9999).ToString();

            _dbMigrator.Migrate(_dbConnection, schema, scripts);

            string expected = String.Format("SELECT * FROM [{0}].MyTable", schema);
            _dbConnection.Received(1).Execute(Arg.Any<string>());
            _dbConnection.Received(1).Execute(expected);
        }

    }
}
