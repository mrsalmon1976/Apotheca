using Apotheca.BLL.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Apotheca.BLL.Repositories
{
    public interface IRepository
    {
        IDbConnection Connection { get; set; }

        IDbTransaction CurrentTransaction { get; set; }

    }

    public class BaseRepository : IRepository
    {

        public BaseRepository(IDbConnection conn, string schema)
        {
            this.Connection = conn;
            this.DbSchema = schema;
        }

        public virtual IDbConnection Connection { get; set; }

        public virtual IDbTransaction CurrentTransaction { get; set; }

        protected virtual string DbSchema { get; private set; }

        //protected virtual string CleanseSchema(string schema)
        //{
        //    Regex rgx = new Regex("[^a-zA-Z0-9]");
        //    return rgx.Replace(schema, "");
        //}

        protected virtual string ReplaceSchemaPlaceholders(string sql)
        {
            return sql.Replace("{SCHEMA}", this.DbSchema);
        }
    }
}
