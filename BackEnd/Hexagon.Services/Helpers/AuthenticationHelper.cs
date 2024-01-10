using Hexagon.Model.Models;
using Hexagon.Services.Interfaces;
using Hexagon.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Hexagon.Services.Helpers
{ 
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> 
    {
        private readonly IHexAuthenticationService _userService;

        public BasicAuthenticationHandler(
            IHexAuthenticationService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username = null;
            UserDTO user = null;
            var a = Thread.CurrentPrincipal;
            try
            {
                var HeaderAuthorization = Request.Headers["Authorization"]
;
                if (HeaderAuthorization.Count()==0)
                    return AuthenticateResult.Fail("Sin usuario");

                var authHeader = AuthenticationHeaderValue.Parse(HeaderAuthorization);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                user = _userService.Get(username, password); 
                if (user == null)

                    return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            Thread.CurrentPrincipal = principal;
             
            return AuthenticateResult.Success(ticket);

            //// skip authentication if endpoint has [AllowAnonymous] attribute
            //var endpoint = Context. ();
            ////if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            ////    return AuthenticateResult.NoResult();

            //if (!Request.Headers.ContainsKey("Authorization"))
            //    return AuthenticateResult.Fail("Missing Authorization Header");

            //UserDTO user = null;
            //try
            //{
            //    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            //    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            //    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            //    var username = credentials[0];
            //    var password = credentials[1];
            //    user =  _userService.Get(username, password);
            //}
            //catch
            //{
            //    return AuthenticateResult.Fail("Invalid Authorization Header");
            //}

            //if (user == null)
            //    return AuthenticateResult.Fail("Invalid Username or Password");

            //var claims = new[] {
            //    new Claim(ClaimTypes.NameIdentifier, user.ÌD),
            //    new Claim(ClaimTypes.Name, user.Name),
            //};
            //var identity = new ClaimsIdentity(claims, Scheme.Name);
            //var principal = new ClaimsPrincipal(identity);
            //var ticket = new AuthenticationTicket(principal, Scheme.Name);

            //return AuthenticateResult.Success(ticket);
        }
    }
}
