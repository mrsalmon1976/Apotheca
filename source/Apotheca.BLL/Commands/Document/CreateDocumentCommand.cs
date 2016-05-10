using Apotheca.BLL.Validators;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apotheca.BLL.Security;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Data;

namespace Apotheca.BLL.Commands.Document
{
    public interface ICreateDocumentCommand : ICommand<Guid>
    {
        DocumentEntity Document { get; set; }
    }

    public class CreateDocumentCommand : Command<Guid>, ICreateDocumentCommand
    {
        private IDbContext _dbContext;
        private IDocumentValidator _documentValidator;
        private IDocumentRepository _documentRepo;

        public CreateDocumentCommand(IDbContext dbContext, IDocumentValidator documentValidator, IDocumentRepository documentRepo)
        {
            _dbContext = dbContext;
            _documentValidator = documentValidator;
            _documentRepo = documentRepo;
        }

        public DocumentEntity Document { get; set; }
        
        public override Guid Execute()
        {
            if (this.Document == null) throw new NullReferenceException("Document property cannot be null");

            //validate
            _documentValidator.Validate(this.Document);

            // start a transaction as now we're hitting the database
            IDbTransaction txn = this._dbContext.BeginTransaction();
            try
            {
                // set the CreatedOn and insert the new document
                this.Document.CreatedOn = DateTime.UtcNow;
                _documentRepo.Create(this.Document);

                txn.Commit();
            }
            catch (Exception)
            {
                if (txn != null) txn.Rollback();
                throw;
            }
            return this.Document.Id.Value;
        }

    }
}
