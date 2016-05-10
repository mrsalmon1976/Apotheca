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
    public interface IDocumentValidator
    {
        void Validate(DocumentEntity document);
    }

    public class DocumentValidator : IDocumentValidator
    {
        public DocumentValidator()
        {
        }

        public void Validate(DocumentEntity document)
        {
            if (document == null) throw new ArgumentNullException("document");

            List<string> errors = new List<string>();

            if (String.IsNullOrWhiteSpace(document.FileName))
            {
                errors.Add("File name not supplied");
            }
            if (String.IsNullOrWhiteSpace(document.Extension))
            {
                errors.Add("Extension not supplied");
            }
            if (document.FileContents.Length == 0)
            {
                errors.Add("File contents not supplied");
            }
            if (document.CreatedByUserId == Guid.Empty)
            {
                errors.Add("User not supplied");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

        }
    }
}
