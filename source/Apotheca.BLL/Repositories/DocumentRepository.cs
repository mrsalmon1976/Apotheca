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
    public interface IDocumentRepository
    {
        Task<int> GetCountAsync();

        void Create(DocumentEntity user);

        DocumentEntity GetById(Guid id);

    }

    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public DocumentRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public void Create(DocumentEntity user)
        {
            throw new NotImplementedException();
//            IDbConnection conn = this.DbContext.GetConnection();
//            string sql = this.ReplaceSchemaPlaceholders(@"
//                DECLARE @returnid TABLE (id uniqueidentifier);
//                INSERT INTO [{SCHEMA}].[Users] 
//                (Email, Password, Salt, FirstName, Surname, Role, ApiKey, CreatedOn) 
//                output inserted.id into @returnid
//                VALUES
//                (@Email, @Password, @Salt, @FirstName, @Surname, @Role, @ApiKey, @CreatedOn);
//                select r.id from @returnid r");
//            Guid id = conn.ExecuteScalar<Guid>(sql, user, transaction: DbContext.CurrentTransaction);
//            user.Id = id;
        }

        public DocumentEntity GetById(Guid id)
        {
            throw new NotImplementedException();
            //string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Users] where Id = @Id");
            //return this.DbContext.GetConnection().Query<UserEntity>(sql, new { Id = id }).SingleOrDefault();
        }

        public async Task<int> GetCountAsync()
        {
            using (IDbConnection conn = this.DbContext.GetConnection())
            {
                string sql = this.ReplaceSchemaPlaceholders("SELECT COUNT(*) FROM [{SCHEMA}].[Documents]");
                Task<int> count = conn.ExecuteScalarAsync<int>(sql);
                return await count;
            }
        }

    }
}
