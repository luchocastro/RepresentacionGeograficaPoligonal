using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hexagon.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hexagon.Shared.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IFileService FileService;
        public FileController(IFileService IFileService)
        {
            this.FileService = IFileService;
        }

        //DataFileConfigurationDTO
        [HttpGet]
        public IActionResult GetDataFileConfigurations()
        {
            return Ok(FileService.GetDataFileConfiguration());
        }

        [HttpPost]
        public IActionResult Upload(FilePost FilePost)
        {
            if (FilePost.Base64File == null || FilePost.Base64File == "") return BadRequest();
            NativeJsonFileDTO NativeJsonFileDTO = FileService.ConvertFile(FilePost.Base64File, FilePost.FileData);
            return Ok(NativeJsonFileDTO);
        }


    }
}
