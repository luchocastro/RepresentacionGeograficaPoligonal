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
        
        public int OriginalPosition { get; set; }
        public EnumActionToDoWithUncastedDTO ActionToDoWithUncasted { get; set; }
        public string DirectoryOfColumns { get; set; }
        public bool IsPolygon { get; set; } = false;

        public string FileName { get; set; }
        public string PathFields { get; set; }
        public string PathSampleFields { get; set; }
        public Dictionary<EnumAlowedDataTypeDTO, int> DictionaryEnumAlowedDataType { get; set; }
        public object MaxValue { get; set; } = null;
        public object MinValue { get; set; } = null;
        public long NumberOfRows { get; set; } = 0;
    }
}
