using Apotheca.BLL.Commands.Category;
using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Data;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Services;
using Apotheca.Validators;
using Apotheca.ViewModels;
using Apotheca.ViewModels.Category;
using Apotheca.ViewModels.Document;
using Apotheca.Web.Results;
using AutoMapper;
using Nancy;
using System;
using System.Collections.Generic;

namespace Apotheca.Controllers
{
    public interface ICategoryController
    {
        IControllerResult HandleCategoryGet();

        IControllerResult HandleCategoryPost(CategoryEntity category);
    }

    public class CategoryController : ICategoryController
    {
        private IUnitOfWork _unitOfWork;
        private ISaveCategoryCommand _saveCategoryCommand;

        public CategoryController(IUnitOfWork unitOfWork, ISaveCategoryCommand saveCategoryCommand)
        {
            _unitOfWork = unitOfWork;
            _saveCategoryCommand = saveCategoryCommand;
        }

        public IControllerResult HandleCategoryGet()
        {
            CategoryViewModel model = new CategoryViewModel();
            model.Categories.AddRange(_unitOfWork.CategoryRepo.GetAll());
            return new ViewResult(Views.Category.Form, model);
        }


        public IControllerResult HandleCategoryPost(CategoryEntity category)
        {

            // try and execute the command 
            BasicResult result = new BasicResult(true);
            try
            {
                _unitOfWork.BeginTransaction();
                _saveCategoryCommand.Category = category;
                _saveCategoryCommand.Execute();
                _unitOfWork.Commit();
            }
            catch (ValidationException vex)
            {
                result = new BasicResult(false, vex.Errors.ToArray());
            }
            catch (Exception ex)
            {
                result = new BasicResult(false, ex.Message);
            }
                        
            return new JsonResult(result);
        }

    }
}
