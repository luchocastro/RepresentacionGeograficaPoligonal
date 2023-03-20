using Hexagon.Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class HexagonDetailsDTO : BaseDto
    {


        public List<ColumnDTO> Columns { get; set; }

    }
}
