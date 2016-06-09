using Apotheca.BLL.Data;
using Apotheca.BLL.Models;
using Apotheca.BLL.Validators;
using System;

namespace Apotheca.BLL.Commands.Category
{
    public interface ISaveCategoryCommand : ICommand<Guid>
    {
        CategoryEntity Category { get; set; }
    }

    public class SaveCategoryCommand : Command<Guid>, ISaveCategoryCommand
    {
        private IUnitOfWork _unitOfWork;
        private ICategoryValidator _categoryValidator;

        public SaveCategoryCommand(IUnitOfWork unitOfWork, ICategoryValidator categoryValidator)
        {
            _unitOfWork = unitOfWork;
            _categoryValidator = categoryValidator;
        }

        public CategoryEntity Category { get; set; }
        
        public override Guid Execute()
        {
            if (this.Category == null) throw new NullReferenceException("Category property cannot be null");
            if (_unitOfWork.CurrentTransaction == null) throw new InvalidOperationException("SaveCategoryCommand must be executed as part of a transaction");

            bool isExisting = this.Category.Id.HasValue;

            // validate
            _categoryValidator.Validate(this.Category);

            if (isExisting)
            {
                _unitOfWork.CategoryRepo.Update(this.Category);
            }
            else
            {
                this.Category.CreatedOn = DateTime.UtcNow;
                _unitOfWork.CategoryRepo.Create(this.Category);
            }

            return this.Category.Id.Value;
        }

    }
}
