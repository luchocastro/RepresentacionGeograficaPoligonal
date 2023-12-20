using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class FileToAnalizePost
    {
        public DataFileConfigurationDTO DataFileConfiguration { get; set; }
        public string HexafileID { get; set; }
        public int NRows { get; set; }
    }
}
