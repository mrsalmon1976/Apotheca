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
    public interface ICategoryRepository : IRepository
    {
        void Create(CategoryEntity category);
        IEnumerable<CategorySearchResult> GetAll();
        CategoryEntity GetByNameOrDefault(string name);
        void Update(CategoryEntity category);
    }

    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IDbConnection conn, string schema)
            : base(conn, schema)
        {
        }

        public void Create(CategoryEntity category)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
            DECLARE @returnid TABLE (id uniqueidentifier);
            INSERT INTO [{SCHEMA}].[Categories] 
            (Name, Description, CreatedOn) 
            output inserted.id into @returnid
            VALUES
            (@Name, @Description, @CreatedOn);
            select r.id from @returnid r");
            Guid id = this.Connection.ExecuteScalar<Guid>(sql, category, transaction: this.CurrentTransaction);
            category.Id = id;
        }

        public IEnumerable<CategorySearchResult> GetAll()
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                SELECT Id AS CategoryId, Name, Description, CreatedOn
                    , (SELECT COUNT(DISTINCT DocumentId) FROM [{SCHEMA}].DocumentCategoryAssc WHERE CategoryId = c.Id) AS DocumentCount
                FROM [{SCHEMA}].[Categories] c
                ORDER BY Name");
            return this.Connection.Query<CategorySearchResult>(sql, this.CurrentTransaction);
        }

        public CategoryEntity GetByNameOrDefault(string name)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Categories] where Name = @Name");
            return this.Connection.Query<CategoryEntity>(sql
                , new { Name = name }
                , this.CurrentTransaction).SingleOrDefault();
        }

        public void Update(CategoryEntity category)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                UPDATE [{SCHEMA}].[Categories] 
                SET Name = @Name, Description = @Description
                    , CreatedOn = @CreatedOn
                    WHERE Id = @Id");
            this.Connection.Execute(sql, category, transaction: this.CurrentTransaction);
        }

    }
}
