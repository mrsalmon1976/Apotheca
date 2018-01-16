using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.Data
{
    public enum AppConnection
    {
        Admin,
        Default
    }

    public interface IDbContextFactory
    {
        IDbContext GetDbContext(AppConnection conn);

        string GetConnectionString(AppConnection conn);
    }
    public class DbContextFactory : IDbContextFactory
    {
        private IConfiguration _config;

        public DbContextFactory(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString(AppConnection conn)
        {
            switch (conn)
            {
                case AppConnection.Admin:
                    return _config.GetConnectionString("AdminConnection");
                default:
                    return _config.GetConnectionString("DefaultConnection");
            }

        }

        public IDbContext GetDbContext(AppConnection conn)
        {
            string connString = this.GetConnectionString(conn);
            return new DbContext(connString);
        }
    }
}
