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
        
        IEnumerable<Guid> Categories { get; set; }
    }

    public class SaveDocumentCommand : Command<Guid>, ISaveDocumentCommand
    {
        private IUnitOfWork _unitOfWork;
        private IDocumentValidator _documentValidator;

        public SaveDocumentCommand(IUnitOfWork unitOfWork, IDocumentValidator documentValidator)
        {
            _unitOfWork = unitOfWork;
            _documentValidator = documentValidator;
        }

        public DocumentEntity Document { get; set; }

        public IEnumerable<Guid> Categories { get; set; }
        
        public override Guid Execute()
        {
            if (this.Document == null) throw new NullReferenceException("Document property cannot be null");
            if (_unitOfWork.CurrentTransaction == null) throw new InvalidOperationException("Command must be executed as part of a transaction");

            bool isExisting = (this.Document.Id != Guid.Empty);

            // determine the version number
            int versionNo = 1;
            if (isExisting)
            {
                versionNo = _unitOfWork.DocumentVersionRepo.GetVersionCount(this.Document.Id) + 1;
            }
            this.Document.VersionNo = versionNo;

            // validate
            _documentValidator.Validate(this.Document);

            // set the CreatedOn and insert the new document
            this.Document.CreatedOn = DateTime.UtcNow;
            if (isExisting)
            {
                _unitOfWork.DocumentRepo.Update(this.Document);
            }
            else
            {
                _unitOfWork.DocumentRepo.Create(this.Document);
            }

            // insert the categories if there are any
            foreach (Guid catId in (this.Categories ?? Enumerable.Empty<Guid>()))
            {
                DocumentCategoryAsscEntity dca = new DocumentCategoryAsscEntity();
                dca.CategoryId = catId;
                dca.DocumentId = this.Document.Id;
                dca.DocumentVersionNo = versionNo;
                _unitOfWork.DocumentCategoryAsscRepo.Create(dca);
            }

            // create the version
            _unitOfWork.DocumentVersionRepo.Create(this.Document);

            return this.Document.Id;
        }

    }
}
