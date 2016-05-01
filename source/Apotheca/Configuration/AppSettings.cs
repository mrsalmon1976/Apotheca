using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Configuration
{
    public interface IAppSettings
    {
        string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the schema to where the Apotheca objects are added in the database.
        /// </summary>
        string DbSchema { get; }
    }

    public class AppSettings : IAppSettings
    {
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString;
            }
        }

        /// <summary>
        /// Gets the name of the schema to where the Apotheca objects are added in the database.
        /// </summary>
        public string DbSchema 
        {
            get
            {
                return ConfigurationManager.AppSettings["DbSchema"];
            }
        }

    }
}
