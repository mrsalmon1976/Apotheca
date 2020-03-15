using Apotheca.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Data
{
    public interface IUnitOfWork : IDisposable
    {
        string DbSchema { get; set; }

        ICategoryRepository CategoryRepo { get; }

        IAuditLogRepository AuditLogRepo { get; }

        IDocumentRepository DocumentRepo { get; }

        IDocumentCategoryAsscRepository DocumentCategoryAsscRepo { get; }

        IDocumentVersionRepository DocumentVersionRepo { get; }

        IUserRepository UserRepo { get; }

        IUserCategoryAsscRepository UserCategoryAsscRepo { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();

        /// <summary>
        /// Gets the current transaction.
        /// </summary>
        IDbTransaction CurrentTransaction { get; }
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDbConnection _conn;
        private IDbTransaction _tran;

        public UnitOfWork(IDbConnection dbConnection, string schema, IAuditLogRepository auditLogRepo, ICategoryRepository categoryRepo, IDocumentRepository documentRepo, IDocumentCategoryAsscRepository documentCategoryAsscRepo, IDocumentVersionRepository documentVersionRepo, IUserRepository userRepo, IUserCategoryAsscRepository userCategoryAsscRepo)
        {
            this._conn = dbConnection;
            this.DbSchema = schema;
            this.AuditLogRepo = auditLogRepo;
            this.CategoryRepo = categoryRepo;
            this.DocumentRepo = documentRepo;
            this.DocumentCategoryAsscRepo = documentCategoryAsscRepo;
            this.DocumentVersionRepo = documentVersionRepo;
            this.UserRepo = userRepo;
            this.UserCategoryAsscRepo = userCategoryAsscRepo;
        }

        /// <summary>
        /// Gets/sets the database schema being used for Apotheca database resources.
        /// </summary>
        public string DbSchema { get; set; }

        public IAuditLogRepository AuditLogRepo { get; private set; }
        public ICategoryRepository CategoryRepo { get; private set; }
        public IDocumentRepository DocumentRepo { get; private set; }
        public IDocumentCategoryAsscRepository DocumentCategoryAsscRepo { get; private set; }
        public IDocumentVersionRepository DocumentVersionRepo { get; private set; }
        public IUserRepository UserRepo { get; private set; }
        public IUserCategoryAsscRepository UserCategoryAsscRepo { get; private set; }

        public IDbTransaction CurrentTransaction
        {
            get
            {
                return _tran;
            }
        }

        public void BeginTransaction()
        {
            _tran = _conn.BeginTransaction();
            this.AuditLogRepo.CurrentTransaction = _tran;
            this.CategoryRepo.CurrentTransaction = _tran;
            this.DocumentRepo.CurrentTransaction = _tran;
            this.DocumentCategoryAsscRepo.CurrentTransaction = _tran;
            this.DocumentVersionRepo.CurrentTransaction = _tran;
            this.UserRepo.CurrentTransaction = _tran;
            this.UserCategoryAsscRepo.CurrentTransaction = _tran;
        }

        public void Commit()
        {
            _tran.Commit();
            _tran = null;
        }

        public void Rollback()
        {
            _tran.Rollback();
            _tran = null;
        }

        public void Dispose()
        {
            if (_conn != null)
            {
                _conn.Dispose();
            }
        }
    }
}
