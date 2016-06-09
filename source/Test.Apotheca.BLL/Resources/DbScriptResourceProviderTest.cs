using Apotheca.BLL.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Apotheca.BLL.Resources
{
    [TestFixture]
    public class DbScriptResourceProviderTest
    {
        private IDbScriptResourceProvider _resourceProvider;

        public DbScriptResourceProviderTest()
        {
        }

        [SetUp]
        public void ResourceServiceTest_SetUp()
        {
            _resourceProvider = new DbScriptResourceProvider();
        }

        /// <summary>
        /// This test doesn't really do anything except ensure that DbMigrations are returned as a hard-coded string 
        /// for the namespace is used to extract the scripts.
        /// </summary>
        [Test]
        public void GetDbMigrationScripts_WhenExecuted_ReturnsScripts()
        {
            IDictionary<string, string> scripts = _resourceProvider.GetDbMigrationScripts();
            Assert.Greater(scripts.Count, 0);
            foreach (KeyValuePair<string, string> kvp in scripts)
            {
                Assert.Greater(kvp.Key.Length, 0);
                Assert.Greater(kvp.Value.Trim().Length, 0);
                Assert.AreNotEqual(kvp.Key, kvp.Value);
            }
        }
    }
}
