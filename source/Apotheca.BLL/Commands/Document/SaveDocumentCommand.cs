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
    public interface ISaveDocumentCommand : ICommand<Guid>
    {
        DocumentEntity Document { get; set; }
    }

    public class SaveDocumentCommand : Command<Guid>, ISaveDocumentCommand
    {
        private IDbContext _dbContext;
        private IDocumentValidator _documentValidator;
        private IDocumentRepository _documentRepo;
        private IDocumentVersionRepository _documentVersionRepo;

        public SaveDocumentCommand(IDbContext dbContext, IDocumentValidator documentValidator, IDocumentRepository documentRepo, IDocumentVersionRepository documentVersionRepo)
        {
            _dbContext = dbContext;
            _documentValidator = documentValidator;
            _documentRepo = documentRepo;
            _documentVersionRepo = documentVersionRepo;
        }

        public DocumentEntity Document { get; set; }
        
        public override Guid Execute()
        {
            if (this.Document == null) throw new NullReferenceException("Document property cannot be null");

            bool isExisting = this.Document.Id.HasValue;

            // start a transaction as now we're hitting the database
            IDbTransaction txn = this._dbContext.BeginTransaction();
            try
            {
                // determine the version number
                int versionNo = 1;
                if (isExisting)
                {
                    versionNo = _documentVersionRepo.GetVersionCount(this.Document.Id.Value) + 1;
                }
                this.Document.VersionNo = versionNo;

                // validate
                _documentValidator.Validate(this.Document);

                // set the CreatedOn and insert the new document
                this.Document.CreatedOn = DateTime.UtcNow;
                if (isExisting)
                {
                    _documentRepo.Update(this.Document);
                }
                else
                {
                    _documentRepo.Create(this.Document);
                }

                // create the version
                _documentVersionRepo.Create(this.Document);

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
