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
using Hexagon.Api.Controllers.VM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private IFileService FileService;
        private IFormulasResumen FormulasResumen;
        public FileController(IFileService IFileService, IConfiguration Configuration, IFormulasResumen FormulasResumen)
        {
            this.FileService = IFileService;
            this.Configuration = Configuration;
            this.FormulasResumen = FormulasResumen;
        }
        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string ProjectName, string UserName)
        {

            try
            {

                var formCollection =  files ;
                var file = formCollection.First();
                var settings = Configuration.Get<Settings>();
                
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var PojectFolder = Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName, UserName);
                    var ProyectDataDTO = FileService.GetProyects(PojectFolder,ProjectName).Where(x =>x.Name == ProjectName).First() ;
                    var fullPath = Path.Combine(ProyectDataDTO.Location.ProyectFolder, ProyectDataDTO.Location.FileFolder, fileName + ".FILE");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync (stream);
                    }
                    //FileService.ConvertFile(fullPath, FilePost.FileData);
                       
                    //
                    ;
                    
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(ProyectDataDTO));
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

        [HttpPost("[action]")]
        public IActionResult ParseFile(ProjectDataPost ProjectDataPost)
        {
            DataFileConfigurationDTO DataFileConfiguration = new DataFileConfigurationDTO();
            DataFileConfiguration.FileType = ProjectDataPost.FileType;
            DataFileConfiguration.FileProperties = ProjectDataPost.FileProperties;
            var ret = FileService.ConvertFile(ProjectDataPost.FileToParse, DataFileConfiguration, new LayoutDto(true, new System.Drawing.PointF(1f, 1f), new System.Drawing.PointF(0f, 1f), 1000));

            return Ok(ret);
        }
        [HttpPost("[action]")]
        public IActionResult GetImageFile(DatosMapaPost DatosMapaPost)
        {

            var ret = FileService.GenerateImge(DatosMapaPost.LayoutDto, DatosMapaPost.PathWithData);
                ;

            return Ok(ret);
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

                var settings = Configuration.Get<Settings>();
            return Ok( FileService.GetDataFileConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settings.ConfigurationFilesName)));
        }
        [HttpGet]
        [Route("Projects")]
        public IActionResult GetProjects(string user)
        {
            return Ok(FileService.GetProyects (Path.Combine (Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName, user)));
        }
        [HttpGet]
        [Route("GetColumns")]
        public IActionResult GetColumns(string path)
        {
            return Ok(FileService.GetFileColumsFromFile(path));
        }
        [HttpGet]
        [Route("Formulas")]
        public IActionResult GetFormulas()
        {
            var settings = Configuration.Get<Settings>();
            return Ok( FormulasResumen.FormulasDisponibles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settings.PathFunctions)));
        }


    }
}
