using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepresentacionWEb.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RepresentacionWEb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration Configuration;
        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration )
        {
            _logger = logger;
            Configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {

            List<DataFileConfiguration> resultado = null;
            List<SelectListItem> ListaTiposArchivos = new List<SelectListItem>();
                using (var client = new HttpClient())
                {
                var settings = Configuration.Get<Constantes>();
                var settingsa = Configuration.GetValue<string>("URLAPI");
                client.BaseAddress = new Uri(settings.URLAPI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("File/GetDataFileConfigurations");
                string apiResponse = await response.Content.ReadAsStringAsync();
                var s = Json(apiResponse);
                resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DataFileConfiguration>>(apiResponse);
                foreach (DataFileConfiguration data in resultado)
                { ListaTiposArchivos.Add(new SelectListItem(data.FileType, data.FileType)); }
            }
            ViewData["TiposArchivos"] = ListaTiposArchivos;
            return View("Index");


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
