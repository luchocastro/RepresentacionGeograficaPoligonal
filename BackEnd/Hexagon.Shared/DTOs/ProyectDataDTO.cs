using Hexagon.Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class ProyectDataDTO : BaseDto
    {

        public LocationDTO Location { get; set; }
        public List<AnalizedFileDTO> AnalizedFiles { get; set; }
    }
}
