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
    public interface IDocumentCategoryAsscRepository : IRepository
    {
        void Create(DocumentCategoryAsscEntity documentCategoryAssc);

        IEnumerable<DocumentCategoryAsscEntity> GetByDocumentVersion(Guid documentId, int versionNo);

    }

    public class DocumentCategoryAsscRepository : BaseRepository, IDocumentCategoryAsscRepository
    {
        public DocumentCategoryAsscRepository(IDbConnection dbConnection, string schema) : base(dbConnection, schema)
        {
        }

        public void Create(DocumentCategoryAsscEntity documentCategoryAssc)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                INSERT INTO [{SCHEMA}].[DocumentCategoryAssc] 
                (DocumentId, DocumentVersionNo, CategoryId) 
                VALUES
                (@DocumentId, @DocumentVersionNo, @CategoryId);
                SELECT SCOPE_IDENTITY()");
            int id = this.Connection.ExecuteScalar<int>(sql, documentCategoryAssc, transaction: this.CurrentTransaction);
            documentCategoryAssc.Id = id;
        }

        public IEnumerable<DocumentCategoryAsscEntity> GetByDocumentVersion(Guid documentId, int versionNo)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[DocumentCategoryAssc] where DocumentId = @DocumentId AND DocumentVersionNo = @VersionNo");
            return this.Connection.Query<DocumentCategoryAsscEntity>(sql, new { DocumentId = documentId, VersionNo = versionNo }); 
        }



    }
}
