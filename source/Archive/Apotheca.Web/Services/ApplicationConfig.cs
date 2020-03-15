using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.Services
{
    public interface IApplicationConfig
    {
        /// <summary>
        /// Gets whether database migrations should be run on startup.
        /// </summary>
        bool AutoRunMigrations { get; }
    }
    public class ApplicationConfig : IApplicationConfig
    {
        private IConfiguration _config;

        public ApplicationConfig(IConfiguration config)
        {
            _config = config;
        }
        public bool AutoRunMigrations
        {
            get
            {
                return _config.GetValue<bool>("AutoRunMigrations");
            }
            
        }
    }
}
