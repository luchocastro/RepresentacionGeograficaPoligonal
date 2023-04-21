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
using Hexagon.Model.FileDataManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hexagon.Api.Controllers
{
    /// <summary>
    /// Maneja el corazòn de la app. Està algo sobre cargada, candidata al refactoreo
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes
            = "BasicAuthentication")]
          
    public class FileController : ControllerBase
    {
        string UserName;
        private readonly IConfiguration Configuration;
        private IFileService FileService;
        private IFormulasResumen FormulasResumen;
        private readonly IMapper Mapper;
        private readonly IFileDataManagerOptions FileDataManagerOptions;
        /// <summary>
        /// Ya cuatro servicios de movida es mucho. Creo que uno va a llevar los archivos y la otro va a calcular y dibujar
        /// </summary>
        /// <param name="IFileService"></param>
        /// <param name="Configuration"></param>
        /// <param name="FormulasResumen"></param>
        /// <param name="Mapper"></param>
        public FileController(IFileService IFileService, IConfiguration Configuration,
            IFormulasResumen FormulasResumen, IFileDataManagerOptions IFileDataManagerOptions)
        {
            UserName= "";
            this.FileService = IFileService;
            this.Configuration = Configuration;
            this.FormulasResumen = FormulasResumen;
            IFileDataManagerOptions = FileDataManagerOptions;

        }
        /// <summary>
        /// Sue el archivo y crea el primer proyecto si no hay y si hay , agrega un dataset
        /// </summary>
        /// <param name="files"></param>
        /// <param name="ProjectName"></param>
        /// <param name="DataSetName"></param>
        /// <returns></returns>
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
                     
                    var fileDTO = FileService.PutFileAsync(UserName,ProjectName,Nic, file) ;

                    

                    //FileService.ConvertFile(fullPath, FilePost.FileData);
                       
                    //
                    ;
                    
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(fileDTO));
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
        public IActionResult GenerateImage(string PaletteClassID, string CalculatedHexagonID)
        {



            var ret = FileService.GenerateImge(PaletteClassID,  CalculatedHexagonID);

            return Ok(ret);
        }
        /// <summary>
        /// Parse el archivo ya subido
        /// </summary>
        /// <param name="ProjectDataPost"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult DoCalc(DoCalcDataPost DoCalcDataPost)
        {



            var ret = FileService.DoCalc(DoCalcDataPost.FunctionID, DoCalcDataPost.Columns);

            return Ok(ret);
        }
        /// <summary>
        /// Parse el archivo ya subido
        /// </summary>
        /// <param name="ProjectDataPost"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult ParseFileColumns(ProjectDataPost ProjectDataPost)
        {
            DataFileConfigurationDTO DataFileConfiguration = ProjectDataPost.DataFileConfiguration;

                
                var ret = FileService.ConvertFileAsync  (DataFileConfiguration , ProjectDataPost.HexFileID);

            return Ok(ret);
          }
        /// <summary>
        /// Esta es magia pura, aunque eso de los vectores me tiene loco
        /// </summary>
        /// <param name="DatosMapaPost"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult GetImageFile(DatosMapaPost DatosMapaPost)
        {

            var ret = FileService.GenerateLayout (DatosMapaPost.LayoutDto, DatosMapaPost.PathWithData);
                ;

            return Ok(ret);
        }
        [HttpPost]
        [Route("SetFunction")]

        public IActionResult SetFunction(FunctionDataPost functionDataPost)
        {
            return Ok(FileService.SetFunction(functionDataPost.ParentID, functionDataPost.Function));
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }
        /// <summary>
        /// Devuelve la configuracion de datos que puede leer el sistema
        /// </summary>
        /// <returns></returns>
            //DataFileConfigurationDTO

            [HttpGet]
        [Route("GetDataFileConfigurations")]
        
        public IActionResult GetDataFileConfigurations()
        {

                var settings = Configuration.Get<Settings>();
            return Ok( FileService.GetDataFileConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settings.ConfigurationFilesName)));
        }
        /// <summary>
        /// Devuelve los proyectos del usuario logueado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Projects")]
        public IActionResult GetProjects()
        {
            string user = User.Identity.Name;
            return Ok(FileService.GetProyects (user));
        }
        [HttpGet]
        [Route("DataToAnalize")]
        public IActionResult GetAnalizedFiles(string ProyectID)
        {
            /// ProyectDataDTO ProyectDataDTO = new ProyectDataDTO();
            return Ok(FileService.GetAnalizedFiles (ProyectID));
        }
        [HttpGet]
        [Route("FilesToGet")]
        public IActionResult GetFiles(string AnalizedFileID)
        {
            return Ok(FileService.GetHexFiles(AnalizedFileID));
        }
        /// <summary>
        /// devuelve las columnas de un archivo con su Tipo
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetColumns")]
        public IActionResult GetColumns(FileToAnalizePost FileToAnalizePost)
        {
            return Ok(FileService.GetFileColumsFromFile (FileToAnalizePost.DataFileConfiguration, FileToAnalizePost.HexafileID, FileToAnalizePost.NRows));
        }
        /// <summary>
        /// devuelve las fòrmulas y los paràmetros de las mismas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Formulas")]
        public IActionResult GetFormulas()
        {
            var settings = Configuration.Get<Settings>();
            return Ok( FormulasResumen.FormulasDisponibles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settings.PathFunctions)));
        }
         
    }
}
