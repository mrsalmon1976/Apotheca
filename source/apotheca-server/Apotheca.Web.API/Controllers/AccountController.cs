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
        private readonly IAppSettings _appSettings;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IAccountViewModelService _accountViewModelService;

        public AccountController(IAppSettings appSettings, IAuthService authService, IUserService userService, IAccountViewModelService accountViewModelService)
        {
            this._appSettings = appSettings;
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
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<User>(registerViewModel);
                //await _userService.CreateUser(user);

                RegionEndpoint region = RegionEndpoint.GetBySystemName(_appSettings.CognitoSettings.Region);
                var cognito = new AmazonCognitoIdentityProviderClient(region);

                var clientId = _appSettings.CognitoSettings.AppClientId;
                var clientSecretId = _appSettings.CognitoSettings.AppClientSecret; 

                var request = new SignUpRequest
                {
                    ClientId = clientId,
                    SecretHash = CognitoHashCalculator.GetSecretHash(user.Email, clientId, clientSecretId),
                    Username = user.Email,
                    Password = user.Password,
                };

                var emailAttribute = new AttributeType
                {
                    Name = "email",
                    Value = user.Email
                };
                request.UserAttributes.Add(emailAttribute);

                var response = await cognito.SignUpAsync(request);
                return Ok();

                //return Unauthorized("No user found matching the supplied email address/password");
            }
            else
            {
                return this.ValidationProblem(this.ModelState);
            }

        }

    }
}

