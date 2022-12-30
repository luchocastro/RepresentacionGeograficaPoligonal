using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers
{
    public class FilePost
    {

        public string PathFile { get; set; }
        public string ProjectName { get; set; }
        public string UserName { get; set; }
        public List<Microsoft.AspNetCore.Http.IFormFile> files { get; set; }

    }
}
