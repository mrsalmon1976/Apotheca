using Apotheca.BLL.Models;
using Apotheca.BLL.Validators;
using Apotheca.ViewModels.Document;
using Apotheca.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.Validators
{
    public interface IDocumentViewModelValidator
    {
        List<string> Validate(DocumentViewModel model);
    }

    public class DocumentViewModelValidator : IDocumentViewModelValidator
    {
        public DocumentViewModelValidator()
        {
        }

        public List<string> Validate(DocumentViewModel model)
        {
            if (model == null) throw new ArgumentNullException("model");

            List<string> errors = new List<string>();

            if (String.IsNullOrWhiteSpace(model.FileName))
            {
                errors.Add("File name not supplied");
            }
            if (String.IsNullOrWhiteSpace(model.UploadedFileName))
            {
                errors.Add("Uploaded file name not supplied");
            }

            return errors;
        }
    }
}
