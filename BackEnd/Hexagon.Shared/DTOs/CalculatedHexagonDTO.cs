using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    
    public struct HexaDetailWithValueDTO
    {
        public HexagonPositionDTO HexagonPosition { get; set; }
        public float Value { get; set; }
    }
    public class CalculatedHexagonDTO : Base.BaseDto
    {
        public string Name { get; set; }
        public List<string> ColumnNamesForFunction { get; set; }
        public List<HexaDetailWithValueDTO> HexaDetailWithValue { get; set; }
    }
}
