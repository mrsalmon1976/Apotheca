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
    public interface ICategoryRepository
    {
        void Create(CategoryEntity user);
        IEnumerable<CategorySearchResult> GetAll();
        CategoryEntity GetByNameOrDefault(string name);

    }

    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IDbContext dbContext)
            : base(dbContext)
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
            Guid id = this.Connection.ExecuteScalar<Guid>(sql, category, transaction: DbContext.CurrentTransaction);
            category.Id = id;
        }

        public IEnumerable<CategorySearchResult> GetAll()
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                SELECT Id AS CategoryId, Name, Description, CreatedOn
                    , (SELECT COUNT(Id) FROM [{SCHEMA}].Documents WHERE CategoryId = c.Id) AS DocumentCount
                FROM [{SCHEMA}].[Categories] c
                ORDER BY Name");
            return this.DbContext.GetConnection().Query<CategorySearchResult>(sql, this.DbContext.CurrentTransaction);
        }

        public CategoryEntity GetByNameOrDefault(string name)
        {
            string sql = this.ReplaceSchemaPlaceholders("select * from [{SCHEMA}].[Categories] where Name = @Name");
            return this.DbContext.GetConnection().Query<CategoryEntity>(sql
                , new { Name = name }
                , this.DbContext.CurrentTransaction).SingleOrDefault();
        }
    }
}
