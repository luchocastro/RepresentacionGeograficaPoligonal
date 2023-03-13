using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Model
{
    public enum EnumFileType
    {
        [Description("Original")]
        Original = 1,
            [Description("Parsed")]
        Parsed,
            [Description("MapFile")]
        MapFile,
        [Description("HexaFile")]
        HexaFile

    }
}
