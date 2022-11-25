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

namespace RepresentacionWEb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentacionController : Controller
    {
        public async Task<IActionResult> Index()
        {
            
            return View("Index");

        }
        public async Task<IActionResult> CurrentMap()
        {

            return View("Index");

        }
    }
}
