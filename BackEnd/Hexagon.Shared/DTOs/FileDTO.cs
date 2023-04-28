using Hexagon.Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class HexFileDTO : BaseDto
    {

        public string FileName { get; set; }
        public string PahtFile { get; set; }
        public string TypeFile { get; set; }
    }
}
