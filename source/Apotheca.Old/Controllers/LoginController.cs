using Apotheca.BLL.Repositories;
using Apotheca.Navigation;
using Apotheca.Modules;
using Apotheca.ViewModels.Login;
using Apotheca.Web.Results;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apotheca.Web;
using Nancy.Security;
using Apotheca.BLL.Data;
using Apotheca.BLL.Models;
using Apotheca.BLL.Security;
using Apotheca.ViewModels;

namespace Apotheca.Controllers
{
    public interface ILoginController
    {
        IControllerResult LoginGet(IUserIdentity currentUser, LoginViewModel model);

        IControllerResult LoginPost(LoginViewModel model);
    }

    public class LoginController : ILoginController
    {
        private IUnitOfWork _unitOfWork;
        private IPasswordProvider _passwordProvider;

        public LoginController(IUnitOfWork unitOfWork, IPasswordProvider passwordProvider)
        {
            _unitOfWork = unitOfWork;
            _passwordProvider = passwordProvider;
        }

        public IControllerResult LoginGet(IUserIdentity currentUser, LoginViewModel model)
        {
            if (!_unitOfWork.UserRepo.UsersExist())
            {
                return new RedirectResult(Actions.Setup.Default);
            }

            if (currentUser != null)
            {
                return new RedirectResult(Actions.Dashboard);
            }

            if (String.IsNullOrEmpty(model.ReturnUrl))
            {
                model.ReturnUrl = Actions.Dashboard;
            }
            return new ViewResult(Views.Login, model);
        }

        public IControllerResult LoginPost(LoginViewModel model)
        {
            LoginResult loginResult = new LoginResult();

            // if the email or password hasn't been supplied, exit
            if ((String.IsNullOrWhiteSpace(model.Email)) || (String.IsNullOrWhiteSpace(model.Password)))
            {
                return loginResult;
            }

            // get the user
            UserEntity user = _unitOfWork.UserRepo.GetUserByEmailOrDefault(model.Email);
            if (user != null && _passwordProvider.CheckPassword(model.Password, user.Password))
            {
                loginResult.Success = true;
                loginResult.UserId = user.Id;
            }
            
            return loginResult;
        }

    }
}
