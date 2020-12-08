using System;
using System.Collections.Generic;
using System.Text;
using static Hexagon.Shared.Enums.EnumDataType;

namespace Hexagon.Shared.DTOs
{
    public class ColumnDataTypeFindedDTO
    {
        public Dictionary< AlowedDataType,int> DataTypeFinded  { get; set; }
        public string Name { get; set; }
    }
}
