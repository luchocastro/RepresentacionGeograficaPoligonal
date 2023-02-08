using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers
{
    public class HexBaseControler : ControllerBase
    {
        public string UserName {get;}
        public string HexBaseControler(string Name, string Pass)
        {
            Request.au
            var HeaderAuthorization = Request.Headers["Authorization"];
            if ( HeaderAuthorization.Count >0)
            {
                UserName =

            }
        }
        
    }
}
