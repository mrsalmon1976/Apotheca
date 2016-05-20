using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Services;
using Apotheca.Validators;
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
    }

    public class CategoryController : ICategoryController
    {
        public CategoryController()
        {
        }

        public IControllerResult HandleCategoryGet()
        {
            CategoryViewModel model = new CategoryViewModel();
            return new ViewResult(Views.Category.Form, model);
        }


    }
}
