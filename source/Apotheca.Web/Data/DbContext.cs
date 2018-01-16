using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.Data
{
    public interface IDbContext
    {
        void BeginTransaction();
    }
    public class DbContext : IDbContext, IDisposable
    {
        private SqlConnection _conn;
        private SqlTransaction _tran;

        public DbContext(string connString)
        {
            _conn = new SqlConnection(connString);
            _conn.Open();
        }

        public void BeginTransaction()
        {
            _tran = _conn.BeginTransaction();
        }

        public void Dispose()
        {
            if (_tran != null)
            {
                _tran.Rollback();
                _tran.Dispose();
            }
            if (_conn != null)
            {
                _conn.Dispose();
            }
        }
    }
}
