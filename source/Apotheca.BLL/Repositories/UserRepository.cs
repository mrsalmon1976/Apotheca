using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Apotheca.BLL.Data;

namespace Apotheca.BLL.Repositories
{
    public interface IUserRepository
    {
        Task<int> GetUserCountAsync();

        bool UsersExist();
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<int> GetUserCountAsync()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT COUNT(*) FROM [{SCHEMA}].[Users]");
            Task<int> count = this.DbContext.GetConnection().ExecuteScalarAsync<int>(sql);
            return await count;
        }

        public bool UsersExist()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT TOP 1 Id FROM [{SCHEMA}].[Users]");
            Guid? id = this.DbContext.GetConnection().ExecuteScalar<Guid?>(sql);
            return id.HasValue;
        }
    }
}
