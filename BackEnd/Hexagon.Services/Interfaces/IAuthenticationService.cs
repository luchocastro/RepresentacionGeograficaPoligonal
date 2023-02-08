using Hexagon.Model.Models;
using Hexagon.Model.Models.Authentication;
using Hexagon.Shared.DTOs;
using Hexagon.Shared.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Services.Interfaces
{
    public interface IHexAuthenticationService
    {
        Task<AuthenticationModel> Authenticate(AuthenticationDto authenticationDto, UserAgentModel userAgentData);
         UserDTO Get(string Name, string Pwd);
         IEnumerable<Claim> GetUserClaims(UserDTO user);
        Task<UserDTO> Authenticate(string Name, string Password);

    }
}
