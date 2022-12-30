using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hexagon.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hexagon.Shared.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Hexagon.Api.Controllers.VM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Signin([FromBody] UserPost User)
        {
            var algo = HttpContext.Items["User"];
            var user = await _authenticationService.Get (User.Username, User.Password);
            return Ok(user);
        }
        private AuthenticationDto prueba(UserPost User)
        {
            return new AuthenticationDto { Username = User.Username, Password = User.Password };
        }

    }
}
