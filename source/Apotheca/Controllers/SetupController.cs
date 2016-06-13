using Apotheca.BLL.Commands.User;
using Apotheca.BLL.Exceptions;
using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Modules;
using Apotheca.Validators;
using Apotheca.ViewModels.Login;
using Apotheca.ViewModels.User;
using Apotheca.Web.Results;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Apotheca.BLL.Data;

namespace Apotheca.Controllers
{
    public interface ISetupController
    {
        IControllerResult DefaultGet();

        IControllerResult DefaultPost(UserViewModel model);

    }

    public class SetupController : ISetupController
    {
        private IUnitOfWork _unitOfWork;
        private ISaveUserCommand _createUserCommand;
        private IUserViewModelValidator _userViewModelValidator;

        public SetupController(IUnitOfWork unitOfWork, ISaveUserCommand createUserCommand, IUserViewModelValidator userViewModelValidator)
        {
            _unitOfWork = unitOfWork;
            _createUserCommand = createUserCommand;
            _userViewModelValidator = userViewModelValidator;
        }

        public IControllerResult DefaultGet()
        {
            if (_unitOfWork.UserRepo.UsersExist())
            {
                return new RedirectResult(Actions.Login.Default);
            }

            UserViewModel model = new UserViewModel();
            return new ViewResult(Views.Setup.Default, model);

        }

        public IControllerResult DefaultPost(UserViewModel model)
        {
            model.Role = Roles.Admin;

            // do first level validation - if it fails then we need to exit
            List<string> validationErrors = this._userViewModelValidator.Validate(model);
            if (validationErrors.Count > 0)
            {
                model.ValidationErrors.AddRange(validationErrors);
                return new ViewResult(Views.Setup.Default, model);
            }

            UserEntity user = Mapper.Map<UserViewModel, UserEntity>(model);
            // try and execute the command 
            try
            {
                _unitOfWork.BeginTransaction();
                _createUserCommand.User = user;
                _createUserCommand.Execute();
                _unitOfWork.Commit();
            }
            catch (ValidationException vex)
            {
                model.ValidationErrors.AddRange(vex.Errors);
                return new ViewResult(Views.Setup.Default, model);
            }

            // if we've got here, we're all good - redirect to the dashboard
            return new LoginAndRedirectResult(model.Id, Actions.Dashboard);
        }
    }
}
