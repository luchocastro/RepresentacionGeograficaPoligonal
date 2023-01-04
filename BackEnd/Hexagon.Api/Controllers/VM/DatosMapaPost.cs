using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class DatosMapaPost
    {
        [JsonPropertyName("LayoutDto")]
        public LayoutDto LayoutDto { get; set; }
        public String PathWithData { get; set; }
    }
}
