using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Apotheca.BLL.Models;
using Apotheca.BLL.Security;
using Apotheca.BLL.Services;
using Apotheca.Web.API.Config;
using Apotheca.Web.API.Services;
using Apotheca.Web.API.ViewModels;
using Apotheca.Web.API.ViewModels.Account;
using Apotheca.Web.API.ViewModels.Common;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Apotheca.Web.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAmazonCognitoProvider _cognitoProvider;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IAccountViewModelService _accountViewModelService;

        public AccountController(IAmazonCognitoProvider cognitoProvider, IAuthService authService, IUserService userService, IAccountViewModelService accountViewModelService)
        {
            this._cognitoProvider = cognitoProvider;
            this._authService = authService;
            this._userService = userService;
            this._accountViewModelService = accountViewModelService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var authenticatedUser = await _authService.Authenticate(login.Email, login.Password);
                if (authenticatedUser != null)
                {
                    // if registration has not been completed, reject
                    if (!authenticatedUser.RegistrationCompleted.HasValue)
                    {
                        return Unauthorized("Registration has not been completed for this account");
                    }

                    // send back the user account
                    UserViewModel userViewModel = await _accountViewModelService.LoadUserWithStores(authenticatedUser);
                    return Ok(userViewModel);
                }

                return Unauthorized("No user found matching the supplied email address/password");
            }
            else
            {
                return this.ValidationProblem(this.ModelState);
            }

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            User user = Mapper.Map<User>(registerViewModel);

            await _cognitoProvider.RegisterAsync(user.Email, user.Password);
            return Ok();
        }

    }
}

