using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Data
{
    public interface IDbContext : IDisposable
    {
        IDbTransaction BeginTransaction();

        IDbConnection GetConnection();

        IDbTransaction CurrentTransaction { get; }

        string Schema { get; set; }
    }

    public class DbContext : IDbContext, IDisposable
    {
        private string _connectionString;
        private IDbConnection _conn;
        private Object _connLock = new Object();

        public DbContext(string connectionString, string schema)
        {
            _connectionString = connectionString;
            this.Schema = schema;
        }

        public virtual IDbTransaction BeginTransaction()
        {
            IDbTransaction txn = this.GetConnection().BeginTransaction();
            this.CurrentTransaction = txn;
            return txn;
        }

        public virtual IDbTransaction CurrentTransaction { get; private set; }

        /// <summary>
        /// Gets/sets the schema used in the database (configurable app setting)
        /// </summary>
        public virtual string Schema { get; set; }

        public virtual IDbConnection GetConnection()
        {
            lock (_connLock)
            {
                if (_conn == null)
                {
                    _conn = new SqlConnection();
                    _conn.ConnectionString = _connectionString;
                    _conn.Open();
                }
            }
            return _conn;
        }

        public void Dispose()
        {
            if (_conn != null) _conn.Dispose();
        }
    }
}
