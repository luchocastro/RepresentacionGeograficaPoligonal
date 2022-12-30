using Hexagon.Model.Models;
using Hexagon.Model.Models.Authentication;
using Hexagon.Services.Interfaces;
using Hexagon.Shared.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Hexagon.Services.Helpers;

namespace Hexagon.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        public AuthenticationService () { }

        public async Task<AuthenticationModel> Authenticate(AuthenticationDto authenticationDto, UserAgentModel userAgentData)
        {

            return  new AuthenticationModel
            {
                Name = authenticationDto.Username
            };
        }


        public async Task<User> Get(string Name, string Pwd)
        {
            return UserHelper.GetUser(Name, Pwd);
        }
    }
}
