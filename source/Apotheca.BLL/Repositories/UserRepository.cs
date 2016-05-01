using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Apotheca.BLL.Repositories
{
    public interface IUserRepository
    {
        Task<int> GetUserCountAsync();

        bool UsersExist();
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        private IDbConnection _conn;

        public UserRepository(string schema, IDbConnection conn) : base(schema)
        {
            _conn = conn;
        }

        public async Task<int> GetUserCountAsync()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT COUNT(*) FROM [{SCHEMA}].[Users]");
            Task<int> count = _conn.ExecuteScalarAsync<int>(sql);
            return await count;
        }

        public bool UsersExist()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT TOP 1 Id FROM [{SCHEMA}].[Users]");
            Guid? id = _conn.ExecuteScalar<Guid?>(sql);
            return id.HasValue;
        }
    }
}
