using Hexagon.Shared.DTOs;
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
        public DataFileConfigurationDTO DataFileConfiguration { get; set; }
        public String  HexFileID { get; set; }
    }
}
