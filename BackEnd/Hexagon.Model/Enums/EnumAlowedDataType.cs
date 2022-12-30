using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Model
{
    public enum EnumAlowedDataType
    {
        [Description("Caracter")]
        Character = 1,
        [Description("Número")]
        GenericNumber,
        [Description("Fecha y hora")]
        DataTime,
        [Description("Posición")]
        ParadeNumeros
    }
}
