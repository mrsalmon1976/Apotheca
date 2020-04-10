using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apotheca.Web.API.ViewModels.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apotheca.Web.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Bearer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ControllerBase
    {

        public DashboardController()
        {
        }

        [HttpGet]
        [Route("userdetails/{userId}")]
        public IActionResult UserDetails(Guid userId)
        {
            UserViewModel user = new UserViewModel() { Email = "test@test.com", FirstName = "Matt", LastName = "Salmon" };
            user.Stores.Add(new StoreViewModel() { Id = Guid.NewGuid(), Name = "Test " });
            //user.Stores.Add()
            return Ok(user);
        }

    }
}
