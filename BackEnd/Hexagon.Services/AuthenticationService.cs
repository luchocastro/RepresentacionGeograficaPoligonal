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
using Hexagon.Model.Repository;
using Hexagon.Shared.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Principal; 


namespace Hexagon.Services
{
    public class HexAuthenticationService : IHexAuthenticationService   , IAuthenticated
    {
        private readonly IDataRepository<UserDTO, User> IDataRepository;
        private UserDTO user = null;
        public HexAuthenticationService(IDataRepository<UserDTO, User> IDataRepository)
        {
            this.IDataRepository = IDataRepository;
            
        }
        public IEnumerable<Claim> GetUserClaims(UserDTO user)
        {
            List<Claim> claims = new List<Claim>();
              
            claims.Add(new Claim(ClaimTypes.Name, user.Name  ));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID));
             
            return claims;
        }
        public async Task<UserDTO> Authenticate(string Name, string Password)
        {
            
            var user = await Task.Run(() =>  IDataRepository.Get(Name));
            return user;
            
        }

        public Task<AuthenticationModel> Authenticate(AuthenticationDto authenticationDto, UserAgentModel userAgentData)
        {
            throw new NotImplementedException();
        }

        public  UserDTO   Get(string Name, string Pwd)
        {
            IDataRepository.Open(Name); 
            UserDTO User = IDataRepository.Get( Name);
            if (User == null)
            {
                User = IDataRepository.Add( new User() { Name = Name, ID= Name, Password= Pwd });
            }
            else
            {
                if (User.Password != Pwd)
                {
                    return null;
                }
            }
            this.user =  new UserDTO
            {
                ID = User.ID ,
                Name = User.Name
            };
            return this.GetUser();
        }

        public UserDTO GetUser( )
        {
            return user;
        }
    }
}
