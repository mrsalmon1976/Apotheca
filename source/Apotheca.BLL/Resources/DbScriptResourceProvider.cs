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
        string[] GetDbMigrationScripts();
    }

    public class DbScriptResourceProvider : IDbScriptResourceProvider
    {
        public const string DbMigrationResourceNamespacePrefix = "Apotheca.BLL.Resources.DbMigrations.";

        public string[] GetDbMigrationScripts()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(name => name.StartsWith(DbMigrationResourceNamespacePrefix));
            List<string> result = new List<string>();

            foreach (string resourceName in resourceNames)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result.Add(reader.ReadToEnd());
                    }
                }
            }

            return result.OrderBy(x => x).ToArray();
        }
    }
}
