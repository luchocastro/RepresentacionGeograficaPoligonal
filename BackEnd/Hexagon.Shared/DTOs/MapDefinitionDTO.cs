using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class MapDefinitionDTO
    {

        public string ColumnNameForX { get; set; }
        public string ColumnNameForY { get; set; }
        public string ColumnForMapGroup { get; set; }
        public List<string> ColumnsNameForFuntion { get; set; }
        public string FunctionName { get; set; }
        public EnumActionToDoWithUncastedDTO ActionToDoWithUncasted { get; }
        public EnumActionToDoWithUncastedDTO EnumActionToDoWithUncastedDTO { get; set; }
        public MapDefinitionDTO()
        { }
        public MapDefinitionDTO(
        
        string _ColumnNameForX,
        string _ColumnNameForY,
        string _ColumnForMapGroup,
        List<string> _ColumnsNameForFuntion,
        string _FunctionName,
        EnumActionToDoWithUncastedDTO EnumActionToDoWithUncastedDTO)
        {

            this.ColumnNameForX = ColumnNameForX;
            this.ColumnNameForY = ColumnNameForY;
            this.ColumnsNameForFuntion = ColumnsNameForFuntion;
            this.ColumnForMapGroup = ColumnForMapGroup;
            this.FunctionName = FunctionName;
            this.EnumActionToDoWithUncastedDTO = EnumActionToDoWithUncastedDTO;
            this.ActionToDoWithUncasted = this.EnumActionToDoWithUncastedDTO;
        }

    }
}
