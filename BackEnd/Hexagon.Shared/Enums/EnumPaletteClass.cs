using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Shared.Enums
{
    public enum EnumPaletteClassDTO
    {
        [Description("Qualitative")]
        Qualitative = 1,
        [Description("Sequential")]
        Sequential = 2,
        [Description("Diverging")]
        Diverging = 3
    }
}
