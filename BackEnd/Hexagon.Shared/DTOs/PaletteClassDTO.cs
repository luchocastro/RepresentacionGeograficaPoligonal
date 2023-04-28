using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class PaletteClassDTO : Base.BaseDto 
    {
        public EnumPaletteClassDTO EnumPaletteClass { get; set; }
        public int MemberNumber { get; set; }

        public Dictionary<int , Color> RGBS { get; set; }  
        public string Palette { get; set; }


    }
}
