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

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes
            = "BasicAuthentication")]
    public class FileController :  HexBaseControler
    {
        string UserName;
        private readonly IConfiguration Configuration;
        private IFileService FileService;
        private IFormulasResumen FormulasResumen;
        private readonly IMapper Mapper;
        private IAuthenticated IAuthenticated;
        public FileController(IFileService IFileService, IConfiguration Configuration, IFormulasResumen FormulasResumen, IMapper Mapper,IAuthenticated IAuthenticated)
        {
            UserName= "";
            this.FileService = IFileService;
            this.Configuration = Configuration;
            this.FormulasResumen = FormulasResumen;
            this.Mapper = Mapper;
            if (HttpContext.Request.Headers. )
        }
        [HttpPost("[action]"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string ProjectName, string DataSetName ="")
        {

            try
            {

                var formCollection =  files ;
                var file = formCollection.First();
                var settings = Configuration.Get<Settings>();
                UserName = User.Identity.Name;
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + ".FILE";
                    var originaName = file.FileName;
                    var Nic =DataSetName==""? originaName:DataSetName ;// Path.GetFileNameWithoutExtension(file.FileName);
                    var AnilizedFile = new AnalizedFileDTO { FileName = fileName, OriginalFileName = originaName, NicName = Nic };
                    var PojectFolder = Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName, UserName);
                    var ProyectDataDTO = FileService.GetProyects(PojectFolder,ProjectName, AnilizedFile).Where(x =>x.Name == ProjectName) ;
                    var Project = ProyectDataDTO.FirstOrDefault(x => x.Name == ProjectName);
                    var fullPath = Path.Combine(Project.Location.ProyectFolder, Nic, Project.Location.FileFolder,
                        fileName);
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

            if (ProjectDataPost.ProyectDataDTO.AnalizedFiles !=null && ProjectDataPost.ProyectDataDTO.AnalizedFiles.Count (x => x.NicName == ProjectDataPost.FileToParse)>0)
            {
                var AnalizedFile = ProjectDataPost.ProyectDataDTO.AnalizedFiles.FirstOrDefault(x => x.NicName == ProjectDataPost.FileToParse);
                var FileToParse = Path.Combine(ProjectDataPost.ProyectDataDTO.Location.ProyectFolder, AnalizedFile.NicName, ProjectDataPost.ProyectDataDTO.Location.FileFolder, AnalizedFile.FileName);
                
                var ret = FileService.ConvertFile(DataFileConfiguration, new LayoutDto(true, new System.Drawing.PointF(1f, 1f), new System.Drawing.PointF(0f, 1f), 1000), ProjectDataPost.ProyectDataDTO, ProjectDataPost.FileToParse);

            return Ok(ret);
          }
            throw new FileNotFoundException();
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
        public IActionResult GetProjects()
        {
            string user = UserName;
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

        public string HexBaseControler(string Name, string Pass)
        {
            throw new NotImplementedException();
        }
    }
}
