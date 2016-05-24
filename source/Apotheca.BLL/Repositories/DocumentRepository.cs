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

        void Create(DocumentEntity document);

        DocumentEntity GetById(Guid id);

        IEnumerable<DocumentSearchResult> Search(string text, IEnumerable<int> categories);

    }

    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public DocumentRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public void Create(DocumentEntity document)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                DECLARE @returnid TABLE (id uniqueidentifier);
                INSERT INTO [{SCHEMA}].[Documents] 
                (FileName, Extension, Description, FileContents, MimeType, CreatedOn, CreatedByUserId) 
                output inserted.id into @returnid
                VALUES
                (@FileName, @Extension, @Description, @FileContents, @MimeType, @CreatedOn, @CreatedByUserId) 
                select r.id from @returnid r");
            Guid id = this.Connection.ExecuteScalar<Guid>(sql, document, transaction: DbContext.CurrentTransaction);
            document.Id = id;
        }

        public DocumentEntity GetById(Guid id)
        {
            throw new NotImplementedException();
            //string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Users] where Id = @Id");
            //return this.DbContext.GetConnection().Query<UserEntity>(sql, new { Id = id }).SingleOrDefault();
        }

        public async Task<int> GetCountAsync()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT COUNT(*) FROM [{SCHEMA}].[Documents]");
            Task<int> count = this.Connection.ExecuteScalarAsync<int>(sql);
            return await count;
        }

        public IEnumerable<DocumentSearchResult> Search(string text, IEnumerable<int> categories)
        {
            string val = String.Format("\"{0}*\"", text);
            string sql = this.ReplaceSchemaPlaceholders(@"
                SELECT TOP 101
                    d.Id AS DocumentId
                    , d.FileName
                    , LTRIM(RTRIM(u.FirstName + ' ' + u.Surname)) AS CreatedBy
                    , d.CreatedOn
                FROM [{SCHEMA}].[Documents] d 
                LEFT JOIN [{SCHEMA}].[Users] u ON d.CreatedByUserId = u.Id 
                WHERE CONTAINS(d.*, @SearchText)
                ");
            return this.Connection.Query<DocumentSearchResult>(sql, new { SearchText = val });
        }


    }
}
