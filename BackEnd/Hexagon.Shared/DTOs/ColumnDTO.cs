using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Hexagon.Shared.DTOs
{
    public class ColumnDTO :Base.BaseDto
    {
        public ColumnDTO() { }
        public ColumnDTO(string Name, int OriginalPosition, EnumActionToDoWithUncastedDTO ActionToDoWithUncasted,
            EnumAlowedDataTypeDTO DataTypeSelected)
        {
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
            this.DataTypeSelected = DataTypeSelected;
        }

        public List<EnumAlowedDataTypeDTO> DataTypeFinded { get; set; }
        public EnumAlowedDataTypeDTO DataTypeSelected { get; set; }
        public string Name { get; set; }
        public int OriginalPosition { get; set; }
        public EnumActionToDoWithUncastedDTO ActionToDoWithUncasted { get; set; }
        
    }
}
