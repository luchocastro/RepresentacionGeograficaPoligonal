using Hexagon.Model.Models;
using Hexagon.Model.Models.Authentication;
using Hexagon.Shared.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> Authenticate(AuthenticationDto authenticationDto, UserAgentModel userAgentData);
        Task<User> Get(string Name, string Pwd);


    }
}
