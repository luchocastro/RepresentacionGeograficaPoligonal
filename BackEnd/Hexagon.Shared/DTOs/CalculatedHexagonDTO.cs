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
        public string LayoutID { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> ColumnNamesForFunction { get; set; } = new List<string>();
        public List<HexaDetailWithValueDTO> HexaDetailWithValue { get; set; } = new List<HexaDetailWithValueDTO>();
    }
}
