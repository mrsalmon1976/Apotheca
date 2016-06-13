using Apotheca.BLL.Commands.User;
using Apotheca.BLL.Commands.Document;
using Apotheca.BLL.Data;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Services;
using Apotheca.Validators;
using Apotheca.ViewModels;
using Apotheca.ViewModels.User;
using Apotheca.ViewModels.Document;
using Apotheca.Web.Results;
using AutoMapper;
using Nancy;
using System;
using System.Collections.Generic;
using Apotheca.ViewModels.Category;

namespace Apotheca.Controllers
{
    public interface IUserController
    {
        IControllerResult HandleUserGet();

        IControllerResult HandleUserGetList();

        IControllerResult HandleUserPost(UserEntity User);
    }

    public class UserController : IUserController
    {
        private IUnitOfWork _unitOfWork;
        private ISaveUserCommand _saveUserCommand;

        public UserController(IUnitOfWork unitOfWork, ISaveUserCommand saveUserCommand)
        {
            _unitOfWork = unitOfWork;
            _saveUserCommand = saveUserCommand;
        }

        public IControllerResult HandleUserGet()
        {
            UserViewModel model = new UserViewModel();
            return new ViewResult(Views.User.Default, model);
        }


        public IControllerResult HandleUserGetList()
        {
            IEnumerable<UserSearchResult> categories = _unitOfWork.UserRepo.GetAllExtended();
            UserListViewModel model = new UserListViewModel();
            model.Users.AddRange(categories);
            return new ViewResult(Views.User.ListPartial, model);
        }

        public IControllerResult HandleUserPost(UserEntity User)
        {

            // try and execute the command 
            BasicResult result = new BasicResult(true);
            try
            {
                _unitOfWork.BeginTransaction();
                _saveUserCommand.User = User;
                _saveUserCommand.Execute();
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
