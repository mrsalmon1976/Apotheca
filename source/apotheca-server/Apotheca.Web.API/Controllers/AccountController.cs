﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apotheca.BLL.Models;
using Apotheca.BLL.Services;
using Apotheca.Web.API.Config;
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
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AccountController(IAuthService authService, IUserService userService)
        {
            this._authService = authService;
            this._userService = userService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var authenticatedUser = Task.Run<User>(() => _authService.Authenticate(login.Email, login.Password)).Result;
                if (authenticatedUser != null)
                {
                    // if registration has not been completed, reject
                    if (!authenticatedUser.RegistrationCompleted.HasValue)
                    {
                        return Unauthorized("Registration has not been completed for this account");
                    }

                    // send back the user account
                    UserViewModel user = Mapper.Map<User, UserViewModel>(authenticatedUser);
                    return Ok(user);
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
        public IActionResult Register([FromBody]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<User>(registerViewModel);
                _userService.CreateUser(user);
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
