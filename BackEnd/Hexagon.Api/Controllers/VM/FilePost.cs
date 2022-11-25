using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers
{
    public class FilePost
    {
        public string Base64File { get; set; }
        public DataFileConfigurationDTO FileData { get; set; }
    }
}
