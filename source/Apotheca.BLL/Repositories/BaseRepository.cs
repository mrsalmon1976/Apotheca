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
        private string _schema;

        public BaseRepository(string schema)
        {
            _schema = CleanseSchema(schema);
        }

        protected virtual string Schema
        {
            get
            {
                return _schema;
            }

        }

        protected virtual string CleanseSchema(string schema)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(schema, "");
        }

        protected virtual string ReplaceSchemaPlaceholders(string sql)
        {
            return sql.Replace("{SCHEMA}", this.Schema);
        }
    }
}
