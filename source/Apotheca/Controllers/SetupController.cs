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

namespace Apotheca.Controllers
{
    public interface ISetupController
    {
        IControllerResult DefaultGet();

        IControllerResult DefaultPost(UserViewModel model);

    }

    public class SetupController : ISetupController
    {
        private IUserRepository _userRepo;
        private ICreateUserCommand _createUserCommand;
        private IUserViewModelValidator _userViewModelValidator;

        public SetupController(IUserRepository userRepo, ICreateUserCommand createUserCommand, IUserViewModelValidator userViewModelValidator)
        {
            _userRepo = userRepo;
            _createUserCommand = createUserCommand;
            _userViewModelValidator = userViewModelValidator;
        }

        public IControllerResult DefaultGet()
        {
            if (_userRepo.UsersExist())
            {
                return new RedirectResult(Actions.Login.Default);
            }

            UserViewModel model = new UserViewModel();
            model.FormAction = Actions.Setup.Default;
            return new ViewResult(Views.Setup.Default, model);

        }

        public IControllerResult DefaultPost(UserViewModel model)
        {
            model.Role = Roles.Admin;
            model.FormAction = Actions.Setup.Default;

            // do first level validation - if it fails then we need to exit
            List<string> validationErrors = this._userViewModelValidator.Validate(model);
            if (validationErrors.Count > 0)
            {
                model.ValidationErrors.AddRange(validationErrors);
                return new ViewResult(Views.Setup.Default, model);
            }

            // try and execute the command 
            try
            {
                UserEntity user = Mapper.Map<UserViewModel, UserEntity>(model);
                _createUserCommand.User = user;
                _createUserCommand.Execute();
            }
            catch (ValidationException vex)
            {
                model.ValidationErrors.AddRange(vex.Errors);
                return new ViewResult(Views.Setup.Default, model);
            }

            // if we've got here, we're all good - redirect to the dashboard
            return new LoginAndRedirectResult(model.Id.Value, Actions.Dashboard);
        }
    }
}
