using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class FunctionDataPost
    {
        public FunctionDTO Function { get; set; }
        public string ParentID { get; set; }
    }
}
