using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hexagon.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hexagon.Shared.DTOs;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Hexagon.Core.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private IFileService FileService;
        public FileController(IFileService IFileService, IConfiguration Configuration)
        {
            this.FileService = IFileService;
            this.Configuration = Configuration;
        }
        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {

            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var settings = Configuration.Get<Settings>();
                var folderName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), settings.PathTempFiles );

                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);

                }
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var fullPath = Path.Combine(folderName, fileName + ".FILE");
                    
                    using (var stream = new FileStream(folderName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { fullPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }
            //DataFileConfigurationDTO
            [HttpGet]
        [Route("GetDataFileConfigurations")]
        public IActionResult GetDataFileConfigurations()
        {
            return Ok(FileService.GetDataFileConfiguration());
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult> Upload([FromBody] FilePost FilePost)
        {
            if (FilePost.Base64File == null || FilePost.Base64File == "") return BadRequest();
            NativeJsonFileDTO NativeJsonFileDTO = FileService.ConvertFile(FilePost.Base64File, FilePost.FileData);

            return Ok(NativeJsonFileDTO);
        }


    }
}
