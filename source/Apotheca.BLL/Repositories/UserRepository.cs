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

        void Create(UserEntity user);
        UserEntity GetUserByEmail(string email);
        UserEntity GetUserById(Guid id);

        bool UsersExist();
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public void Create(UserEntity user)
        {
            IDbConnection conn = this.DbContext.GetConnection();
            string sql = this.ReplaceSchemaPlaceholders(@"
                DECLARE @returnid TABLE (id uniqueidentifier);
                INSERT INTO [{SCHEMA}].[Users] 
                (Email, Password, Salt, FirstName, Surname, Role, ApiKey, CreatedOn) 
                output inserted.id into @returnid
                VALUES
                (@Email, @Password, @Salt, @FirstName, @Surname, @Role, @ApiKey, @CreatedOn);
                select r.id from @returnid r");
            Guid id = conn.ExecuteScalar<Guid>(sql, user, transaction: DbContext.CurrentTransaction);
            user.Id = id;
        }

        public UserEntity GetUserByEmail(string email)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Users] where Email = @Email");
            return this.DbContext.GetConnection().Query<UserEntity>(sql
                , new { Email = email }
                , this.DbContext.CurrentTransaction).SingleOrDefault();
        }

        public UserEntity GetUserById(Guid id)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Users] where Id = @Id");
            return this.DbContext.GetConnection().Query<UserEntity>(sql, new { Id = id }).SingleOrDefault();
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
