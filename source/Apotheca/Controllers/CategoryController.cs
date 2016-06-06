using Apotheca.BLL.Commands.Document;
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
        private ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IControllerResult HandleCategoryGet()
        {
            CategoryViewModel model = new CategoryViewModel();
            model.Categories.AddRange(_categoryRepo.GetAll());
            return new ViewResult(Views.Category.Form, model);
        }


        public IControllerResult HandleCategoryPost(CategoryEntity category)
        {
            
            return new JsonResult(new BasicResult(false, "Sorry, this hasn't been implemented yet"));
        }

    }
}
