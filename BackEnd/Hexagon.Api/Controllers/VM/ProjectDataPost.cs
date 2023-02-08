﻿using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class ProjectDataPost
    {

        public string FileType { get; set; }
        public Dictionary<string, object> FileProperties { get; set; }
        public string FileToParse { get; set; }
        public ProyectDataDTO ProyectDataDTO { get; set; }
    }
}
