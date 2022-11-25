using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Hexagon.Model
{
    public struct ColorPalette
    {
        public ColorPalette(PaletteClass PaletteClass, Color [] Colors)
        {
            this.PaletteClass = PaletteClass;
            this.Colors = Colors;
            
        }
        public PaletteClass PaletteClass { get; }
        public Color [] Colors { get; }
    }
}
