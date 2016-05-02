using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Apotheca.BLL.Data;
using Apotheca.BLL.Models;

namespace Apotheca.BLL.Repositories
{
    public interface IUserRepository
    {
        Task<int> GetUserCountAsync();

        UserEntity GetUserByEmail(string email);
        UserEntity GetUserById(Guid id);

        bool UsersExist();
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public UserEntity GetUserByEmail(string email)
        {
            return this.DbContext.GetConnection().Query<UserEntity>("select * from User where Email = @Email", new { Email = email }).SingleOrDefault();
        }

        public UserEntity GetUserById(Guid id)
        {
            return this.DbContext.GetConnection().Query<UserEntity>("select * from User where Id = @Id", new { Id = id }).SingleOrDefault();
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
