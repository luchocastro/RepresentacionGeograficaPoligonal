using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Shared.Enums
{
    public enum EnumPaletteClassDTO
    {
        [Description("Cualitativa")]
        Qualitative = 1,
        [Description("Sequential")]
        Sequential = 2,
        [Description("Divergente")]
        Diverging = 3
    }
}
