using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apotheca.Auth;
using Apotheca.BLL.Models;
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

        public AccountController(IAmazonCognitoProvider cognitoProvider)
        {
            this._cognitoProvider = cognitoProvider;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            var loginResult = await _cognitoProvider.LoginAsync(login.Email, login.Password);
            var user = await _cognitoProvider.GetUser(loginResult.AccessToken);

            var userViewModel = Mapper.Map<UserViewModel>(user);
            userViewModel.Token = loginResult.IdToken;
            return Ok(userViewModel);
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

            await _cognitoProvider.RegisterAsync(user.Email, user.Password, user.FirstName, user.LastName);
            return Ok();
        }

    }
}

