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

        DocumentEntity GetByIdOrDefault(Guid id, bool includeFileData = false);

        byte[] GetFileContents(Guid id);

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

        public DocumentEntity GetByIdOrDefault(Guid id, bool includeFileData = false)
        {
            string columns = "Id, FileName, Description, Extension, MimeType, CreatedOn, CreatedByUserId";
            if (includeFileData)
            {
                columns += ", FileContents";
            }
            string sql = String.Format(this.ReplaceSchemaPlaceholders("select {0} from [{SCHEMA}].[Documents] where Id = @Id"), columns);
            return this.Connection.Query<DocumentEntity>(sql, new { Id = id }).SingleOrDefault();
        }

        public async Task<int> GetCountAsync()
        {
            string sql = this.ReplaceSchemaPlaceholders("SELECT COUNT(*) FROM [{SCHEMA}].[Documents]");
            Task<int> count = this.Connection.ExecuteScalarAsync<int>(sql);
            return await count;
        }

        public byte[] GetFileContents(Guid id)
        {
            string sql = this.ReplaceSchemaPlaceholders("select FileContents from [{SCHEMA}].[Documents] where Id = @Id");
            return this.Connection.Query<byte[]>(sql, new { Id = id }).Single();
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
