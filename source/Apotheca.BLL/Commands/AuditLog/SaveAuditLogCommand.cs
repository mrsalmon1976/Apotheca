using Apotheca.BLL.Data;
using Apotheca.BLL.Models;
using Apotheca.BLL.Validators;
using System;

namespace Apotheca.BLL.Commands.AuditLog
{
    public interface ISaveAuditLogCommand : ICommand<int>
    {
        AuditLogEntity AuditLog { get; set; }
    }

    public class SaveAuditLogCommand : Command<int>, ISaveAuditLogCommand
    {
        private IUnitOfWork _unitOfWork;
        private IAuditLogValidator _auditLogValidator;

        public SaveAuditLogCommand(IUnitOfWork unitOfWork, IAuditLogValidator auditLogValidator)
        {
            _unitOfWork = unitOfWork;
            _auditLogValidator = auditLogValidator;
        }

        public AuditLogEntity AuditLog { get; set; }
        
        public override int Execute()
        {
            if (this.AuditLog == null) throw new NullReferenceException("AuditLog property cannot be null");
            if (_unitOfWork.CurrentTransaction == null) throw new InvalidOperationException("SaveAuditLogCommand must be executed as part of a transaction");

            // validate
            _auditLogValidator.Validate(this.AuditLog);

            this.AuditLog.AuditDateTime = DateTime.UtcNow;
            _unitOfWork.AuditLogRepo.Create(this.AuditLog);

            return this.AuditLog.Id;
        }

    }
}
