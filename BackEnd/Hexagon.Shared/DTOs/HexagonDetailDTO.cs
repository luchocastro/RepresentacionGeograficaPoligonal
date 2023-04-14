using Hexagon.Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{

    public class HexagonDetailsDTO : BaseDto
    {


        public List<HexagonDetailDTO> List { get; set; }
        public string Name { get; set; }
    }
    public struct HexagonPositionDTO
    {
        public float Q { get; set; }
        public float R { get; set; }
        public float S { get; set; }
        public List<PointDTO> Corners { get; set; }
    }
    public class HexagonDetailDTO
    {
        public long NumOrder { get; set; }

        public List<long> IndexLines
        {
            get; set;
        } = new List<long>();

        public List<HexagonPositionDTO> HexagonPositionForValues { get; set; } = new List<HexagonPositionDTO>();
    }
}

