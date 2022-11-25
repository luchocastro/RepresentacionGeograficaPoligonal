using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Hexagon.Shared.DTOs
{
    public class ColumnDTO
    {
        public ColumnDTO(string Name, int OriginalPosition )
        {
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
        }

        public EnumAlowedDataTypeDTO DataTypeSelected { get; set; }
        public string Name { get; }
        public int OriginalPosition { get; }
        public EnumActionToDoWithUncastedDTO ActionToDoWithUncasted { get; }
        
    }
}
