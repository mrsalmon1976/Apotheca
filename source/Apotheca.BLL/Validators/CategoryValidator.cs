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
    public interface ICategoryValidator
    {
        void Validate(CategoryEntity category);
    }

    public class CategoryValidator : ICategoryValidator
    {
        private IUnitOfWork _unitOfWork;

        public CategoryValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Validate(CategoryEntity category)
        {
            if (category == null) throw new ArgumentNullException("category");

            List<string> errors = new List<string>();

            if (String.IsNullOrWhiteSpace(category.Name))
            {
                errors.Add("Name not supplied");
            }
            else
            {
                // make sure the category doesn't exist already!
                CategoryEntity catDuplicate = _unitOfWork.CategoryRepo.GetByNameOrDefault(category.Name);
                if (catDuplicate != null && catDuplicate.Id != category.Id && catDuplicate.Name == category.Name)
                {
                    errors.Add("Category already exists");
                }
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

        }
    }
}
