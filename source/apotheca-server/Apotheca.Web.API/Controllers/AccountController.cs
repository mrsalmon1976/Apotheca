using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apotheca.BLL.Models;
using Apotheca.BLL.Services;
using Apotheca.Web.API.Config;
using Apotheca.Web.API.ViewModels;
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
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            this._authService = authService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var authenticatedUser = Task.Run<User>(() => _authService.Authenticate(user.Email, user.Password)).Result;
                if (authenticatedUser != null)
                {
                    // remove the password and send back
                    user.Password = null;
                    user.Token = authenticatedUser.Token;
                    return Ok(user);
                }

                return Unauthorized("No user found matching the supplied email address/password");
            }
            else
            {
                return this.ValidationProblem(this.ModelState);
            }

        }

    }
}
