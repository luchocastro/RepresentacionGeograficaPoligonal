using System;
using System.Collections.Generic;
using System.Text;
using Hexagon.Shared.DTOs.Base;
namespace Hexagon.Shared.DTOs.Authentication
{
    public class AuthenticationDto : BaseDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
        public string Username { get; set; }
    }
}
