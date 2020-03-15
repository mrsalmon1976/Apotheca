using Apotheca.BLL.Data;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Validators
{
    public interface IAuditLogValidator
    {
        void Validate(AuditLogEntity auditLog);
    }

    public class AuditLogValidator : IAuditLogValidator
    {
        private IUnitOfWork _unitOfWork;

        public AuditLogValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Validate(AuditLogEntity auditLog)
        {
            if (auditLog == null) throw new ArgumentNullException("auditLog");

            List<string> errors = new List<string>();
            
            if (String.IsNullOrWhiteSpace(auditLog.Action))
            {
                errors.Add("Action not supplied");
            }
            if (String.IsNullOrWhiteSpace(auditLog.Entity))
            {
                errors.Add("Entity not supplied");
            }
            if (String.IsNullOrWhiteSpace(auditLog.Key))
            {
                errors.Add("Key not supplied");
            }
            if (auditLog.UserId == Guid.Empty)
            {
                errors.Add("User Id not supplied");
            }
            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

        }
    }
}
