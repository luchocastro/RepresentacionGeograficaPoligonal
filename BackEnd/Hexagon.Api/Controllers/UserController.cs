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
using Hexagon.Shared.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private IFileService FileService;
        private IFormulasResumen FormulasResumen;
        private readonly IMapper Mapper;
        private  IHexAuthenticationService _authenticationService;

        public UserController(IHexAuthenticationService authenticationService, IFileService IFileService, IConfiguration Configuration, IFormulasResumen FormulasResumen, IMapper Mapper)
        {
            this.FileService = IFileService;
            this.Configuration = Configuration;
            this.FormulasResumen = FormulasResumen;
            this.Mapper = Mapper;
            _authenticationService = authenticationService;
        }

        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate( UserPost userPost)
        {
            var user = _authenticationService.Authenticate(userPost.Username, userPost.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        //public async Task<IActionResult>  Signin(  UserPost user)
        //{ 
        //    var UserLogued = (UserDTO) _authenticationService.Get  (user.Username, user.Password);
            
        //        var identity = new ClaimsIdentity(_authenticationService.GetUserClaims(UserLogued));
        //    var UserProps = new Dictionary<string, string>();
        //    UserProps.Add("Name", UserLogued.Name);
        //    UserProps.Add("ID", UserLogued.ÌD);
        //    var properties = new AuthenticationProperties(UserProps);
        //                ClaimsPrincipal principal = new ClaimsPrincipal(identity);


        //    HttpContext.User.AddIdentity(identity);
        //    await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme  );

        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        //    HttpContext.User.AddIdentity(identity);
        //    var a = HttpContext.User.Identity.IsAuthenticated;
        //     return RedirectToAction("File/Projects?user="+UserLogued.Name );
            
             
        //     ;
        //} 
        private AuthenticationDto prueba(UserPost User)
        {
            return new AuthenticationDto { Username = User.Username, Password = User.Password };
        }
        

    }
}
