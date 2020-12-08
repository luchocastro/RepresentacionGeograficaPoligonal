using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Model
{
    public class EnumDataType
    {
        public enum AlowedDataType {
            [Description("Caracter")]
            Character=1,
            [Description("Número")] 
            GenericNumber,
            [Description("Fecha y hora")] 
            DataTime 
        }
}
}
