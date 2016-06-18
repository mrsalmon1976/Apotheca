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
using System.Linq;

namespace Apotheca.Controllers
{
    public interface IUserController
    {
        IControllerResult HandleUserGet(Guid? userId);

        IControllerResult HandleUserGetList();

        IControllerResult HandleUserPost(UserViewModel model);
    }

    public class UserController : IUserController
    {
        private IUnitOfWork _unitOfWork;
        private ISaveUserCommand _saveUserCommand;
        private IUserViewModelValidator _userViewModelValidator;

        public UserController(IUnitOfWork unitOfWork, ISaveUserCommand saveUserCommand, IUserViewModelValidator userViewModelValidator)
        {
            _unitOfWork = unitOfWork;
            _saveUserCommand = saveUserCommand;
            _userViewModelValidator = userViewModelValidator;
        }

        public IControllerResult HandleUserGet(Guid? userId)
        {
            UserViewModel model = new UserViewModel();
            var categories = this._unitOfWork.CategoryRepo.GetAll();
            var options = categories.Select(x => new MultiSelectItem(x.Id.ToString(), x.Name, true));
            model.CategoryOptions.AddRange(options);
            model.Roles.AddRange(Roles.AllRoles);
            model.IsPermissionPanelVisible = true;
            model.SelectedRole = Roles.User;
            return new ViewResult(Views.User.Default, model);
        }


        public IControllerResult HandleUserGetList()
        {
            IEnumerable<UserSearchResult> categories = _unitOfWork.UserRepo.GetAllExtended();
            UserListViewModel model = new UserListViewModel();
            model.Users.AddRange(categories);
            return new ViewResult(Views.User.ListPartial, model);
        }

        public IControllerResult HandleUserPost(UserViewModel model)
        {

            // do first level validation - if it fails then we need to exit
            List<string> validationErrors = this._userViewModelValidator.Validate(model);
            if (validationErrors.Count > 0)
            {
                var vresult = new BasicResult(false, validationErrors.ToArray());
                return new JsonResult(vresult);
            }

            UserEntity user = Mapper.Map<UserViewModel, UserEntity>(model);
            // try and execute the command 
            BasicResult result = new BasicResult(true);
            try
            {
                _unitOfWork.BeginTransaction();
                _saveUserCommand.User = user;
                _saveUserCommand.CategoryIds = model.CategoryIds;
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
