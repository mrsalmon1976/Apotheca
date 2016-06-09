using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Apotheca.BLL.Data;
using Apotheca.BLL.Models;
using Apotheca.BLL.Exceptions;

namespace Apotheca.BLL.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<int> GetUserCountAsync();

        void Create(UserEntity user);
        UserEntity GetUserByEmail(string email);
        UserEntity GetUserByEmailOrDefault(string email);
        UserEntity GetUserById(Guid id);
        UserEntity GetUserByIdOrDefault(Guid id);

        bool UsersExist();
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbConnection connection, string schema) : base(connection, schema)
        {
        }

        public void Create(UserEntity user)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
            DECLARE @returnid TABLE (id uniqueidentifier);
            INSERT INTO [{SCHEMA}].[Users] 
            (Email, Password, Salt, FirstName, Surname, Role, ApiKey, CreatedOn) 
            output inserted.id into @returnid
            VALUES
            (@Email, @Password, @Salt, @FirstName, @Surname, @Role, @ApiKey, @CreatedOn);
            select r.id from @returnid r");
            Guid id = this.Connection.ExecuteScalar<Guid>(sql, user, transaction: this.CurrentTransaction);
            user.Id = id;
        }

        public UserEntity GetUserByEmail(string email)
        {
            UserEntity user = this.GetUserByEmailOrDefault(email);
            if (user == null) throw new EntityDoesNotExistException("email", email);
            return user;
        }

        public UserEntity GetUserByEmailOrDefault(string email)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Users] where Email = @Email");
            return this.Connection.Query<UserEntity>(sql
                , new { Email = email }
                , this.CurrentTransaction).SingleOrDefault();
        }

        public UserEntity GetUserById(Guid id)
        {
            UserEntity user = this.GetUserByIdOrDefault(id);
            if (user == null) throw new EntityDoesNotExistException("id", id);
            return user;
        }

        public UserEntity GetUserByIdOrDefault(Guid id)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Users] where Id = @Id");
            return this.Connection.Query<UserEntity>(sql, new { Id = id }).SingleOrDefault();
        }

        public async Task<int> GetUserCountAsync()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT COUNT(*) FROM [{SCHEMA}].[Users]");
            Task<int> count = this.Connection.ExecuteScalarAsync<int>(sql);
            return await count;
        }

        public bool UsersExist()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT TOP 1 Id FROM [{SCHEMA}].[Users]");
            Guid? id = this.Connection.ExecuteScalar<Guid?>(sql);
            return id.HasValue;
        }
    }
}
