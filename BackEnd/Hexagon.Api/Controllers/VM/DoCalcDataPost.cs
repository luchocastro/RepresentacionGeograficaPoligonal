﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class DoCalcDataPost
    {
        public string FunctionID { get; set; }
        public List <string>Columns { get; set; }
    }
}