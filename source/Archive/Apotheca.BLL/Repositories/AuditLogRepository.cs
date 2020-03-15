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
    public interface IAuditLogRepository : IRepository
    {
        void Create(AuditLogEntity auditLog);

        Task<IEnumerable<AuditLogDetailModel>> GetLatest(int count);

    }

    public class AuditLogRepository : BaseRepository, IAuditLogRepository
    {
        public AuditLogRepository(IDbConnection dbConnection, string schema)
            : base(dbConnection, schema)
        {
        }

        public void Create(AuditLogEntity auditLog)
        {
            string sql = this.ReplaceSchemaPlaceholders(@"
                INSERT INTO [{SCHEMA}].[AuditLogs] 
                (Entity, [Key], Action, AuditDateTime, UserId, EntityDetail) 
                VALUES
                (@Entity, @Key, @Action, @AuditDateTime, @UserId, @EntityDetail) 
                SELECT SCOPE_IDENTITY()");
            int id = this.Connection.ExecuteScalar<int>(sql, auditLog, transaction: this.CurrentTransaction);
            auditLog.Id = id;
        }


        public async Task<IEnumerable<AuditLogDetailModel>> GetLatest(int count)
        {
            string sql = String.Format(this.ReplaceSchemaPlaceholders(@"
                SELECT TOP {0} al.*
                , LTRIM(RTRIM(ISNULL(u.FirstName, '') + ' ' + ISNULL(u.Surname , ''))) AS UserName
                FROM [{SCHEMA}].[AuditLogs] al
                INNER JOIN [{SCHEMA}].[Users] u ON al.UserId = u.Id
                ORDER BY AuditDateTime DESC"), count);
            Task<IEnumerable<AuditLogDetailModel>> result = this.Connection.QueryAsync<AuditLogDetailModel>(sql, null, transaction: this.CurrentTransaction);
            return await result;
        }

    }
}
