using Apotheca.BLL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Apotheca.BLL.Repositories
{
    public class BaseRepository
    {
        private IDbContext _dbContext;

        public BaseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Schema = CleanseSchema(_dbContext.Schema);
        }

        protected virtual IDbContext DbContext
        {
            get
            {
                return _dbContext;
            }

        }

        protected virtual string CleanseSchema(string schema)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(schema, "");
        }

        protected virtual string ReplaceSchemaPlaceholders(string sql)
        {
            return sql.Replace("{SCHEMA}", this.DbContext.Schema);
        }
    }
}
