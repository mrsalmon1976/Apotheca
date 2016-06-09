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
    public interface IDocumentVersionRepository : IRepository
    {
        void Create(DocumentEntity document);

        DocumentEntity GetByIdOrDefault(Guid id, int versionNo, bool includeFileData = false);

        byte[] GetFileContents(Guid id, int versionNo);

        int GetVersionCount(Guid id);

    }

    public class DocumentVersionRepository : BaseRepository, IDocumentVersionRepository
    {
        public DocumentVersionRepository(IDbConnection dbConnection, string schema) : base(dbConnection, schema)
        {
        }

        public void Create(DocumentEntity document)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                DECLARE @returnid TABLE (id uniqueidentifier);
                INSERT INTO [{SCHEMA}].[DocumentVersions] 
                (Id, VersionNo, FileName, Extension, Description, FileContents, MimeType, CreatedOn, CreatedByUserId) 
                output inserted.id into @returnid
                VALUES
                (@Id, @VersionNo, @FileName, @Extension, @Description, @FileContents, @MimeType, @CreatedOn, @CreatedByUserId) 
                select r.id from @returnid r");
            Guid id = this.Connection.ExecuteScalar<Guid>(sql, document, transaction: this.CurrentTransaction);
            document.Id = id;
        }

        public DocumentEntity GetByIdOrDefault(Guid id, int versionNo, bool includeFileData = false)
        {
            string columns = "Id, VersionNo, FileName, Description, Extension, MimeType, CreatedOn, CreatedByUserId";
            if (includeFileData)
            {
                columns += ", FileContents";
            }
            string sql = String.Format(this.ReplaceSchemaPlaceholders("select {0} from [{SCHEMA}].[DocumentVersions] where Id = @Id AND VersionNo = @VersionNo"), columns);
            return this.Connection.Query<DocumentEntity>(sql, new { Id = id, VersionNo = versionNo }).SingleOrDefault();
        }

        public byte[] GetFileContents(Guid id, int versionNo)
        {
            string sql = this.ReplaceSchemaPlaceholders("select FileContents from [{SCHEMA}].[DocumentVersions] where Id = @Id AND VersionNo = @VersionNo");
            return this.Connection.Query<byte[]>(sql, new { Id = id, VersionNo = versionNo }).Single();
        }

        public int GetVersionCount(Guid id)
        {
            string sql = this.ReplaceSchemaPlaceholders("select Count(Id) from [{SCHEMA}].[DocumentVersions] where Id = @Id");
            return this.Connection.Query<int>(sql, new { Id = id }, this.CurrentTransaction).Single();
        }


    }
}
