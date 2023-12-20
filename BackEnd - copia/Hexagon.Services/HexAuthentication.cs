using Hexagon.Model.Models;
using Hexagon.Model.Repository;
using Hexagon.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Services
{
    public class HexAuthentication : IAuthenticationService
    {

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {

            return Task.FromResult(AuthenticateResult.NoResult());
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            var authorization = context.Request.Headers["Authorization"];

            // 2. If there are no credentials, do nothing.
            if (authorization.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            return AuthenticateAsync(context, scheme);
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {

             return Task.CompletedTask;

        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
             return Task.CompletedTask;
        }


    }
}
