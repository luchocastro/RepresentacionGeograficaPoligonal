using Hexagon.Shared.DTOs.Base;
using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class MapDefinitionDTO : BaseDto
    {

        public ColumnDTO ColumnForX { get; set; }
        public ColumnDTO ColumnForY { get; set; }
        public ColumnDTO ColumnForMapGroup { get; set; }
        public List<string> ColumnsToDiscard { get; set; }
        public List<ColumnDTO> DataTypedColumns { get; set; }

    }
}
