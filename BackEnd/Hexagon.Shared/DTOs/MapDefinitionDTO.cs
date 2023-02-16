using Hexagon.Shared.DTOs.Base;
using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class MapDefinitionDTO : BaseDto
    {

        public string ColumnNameForX { get; set; }
        public string ColumnNameForY { get; set; }
        public string ColumnForMapGroup { get; set; }
        public List<string> ColumnsNameForFuntion { get; set; }
        public string FunctionName { get { return Function != null ? Function.FunctionName:""; }   }
        public FunctionDTO Function { get; set; }
        public EnumActionToDoWithUncastedDTO ActionToDoWithUncasted { get; }
        public EnumActionToDoWithUncastedDTO EnumActionToDoWithUncastedDTO { get; set; }
        public MapDefinitionDTO()
        { }


    }
}
