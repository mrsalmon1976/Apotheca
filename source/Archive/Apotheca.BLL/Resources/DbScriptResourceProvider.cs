using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Resources
{
    public interface IDbScriptResourceProvider
    {
        IDictionary<string, string> GetDbMigrationScripts();
    }

    public class DbScriptResourceProvider : IDbScriptResourceProvider
    {
        public const string DbMigrationResourceNamespacePrefix = "Apotheca.BLL.Resources.DbMigrations.";

        public IDictionary<string, string> GetDbMigrationScripts()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(name => name.StartsWith(DbMigrationResourceNamespacePrefix)).OrderBy(x => x);
            IDictionary<string, string> result = new Dictionary<string, string>();

            foreach (string resourceName in resourceNames)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result.Add(resourceName, reader.ReadToEnd());
                    }
                }
            }

            return result;
        }
    }
}
