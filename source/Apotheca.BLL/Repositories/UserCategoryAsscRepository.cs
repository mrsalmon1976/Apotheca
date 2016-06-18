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
    public interface IUserCategoryAsscRepository : IRepository
    {
        void Create(UserCategoryAsscEntity documentCategoryAssc);

        IEnumerable<UserCategoryAsscEntity> GetByUser(Guid userId);

    }

    public class UserCategoryAsscRepository : BaseRepository, IUserCategoryAsscRepository
    {
        public UserCategoryAsscRepository(IDbConnection dbConnection, string schema) : base(dbConnection, schema)
        {
        }

        public void Create(UserCategoryAsscEntity userCategoryAssc)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                INSERT INTO [{SCHEMA}].[UserCategoryAssc] 
                (UserId, CategoryId) 
                VALUES
                (@UserId, @CategoryId);
                SELECT SCOPE_IDENTITY()");
            int id = this.Connection.ExecuteScalar<int>(sql, userCategoryAssc, transaction: this.CurrentTransaction);
            userCategoryAssc.Id = id;
        }

        public IEnumerable<UserCategoryAsscEntity> GetByUser(Guid userId)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[UserCategoryAssc] where UserId = @UserId");
            return this.Connection.Query<UserCategoryAsscEntity>(sql, new { UserId = userId }); 
        }



    }
}
