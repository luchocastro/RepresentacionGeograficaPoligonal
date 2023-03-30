using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hexagon.Model
{
    public class Column
    {
        public Column()
        { }
            public Column(string Name, int OriginalPosition, EnumActionToDoWithUncasted ActionToDoWithUncasted,
            EnumAlowedDataType DataTypeSelected)
        {
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
            this.DataTypeSelected = DataTypeSelected;
        }
        public List<EnumAlowedDataType> DataTypeFinded { get; set; } = (new EnumAlowedDataType[] { EnumAlowedDataType.Character }).ToList();
        public EnumAlowedDataType DataTypeSelected { get; set; }
        public string Name { get; set; }
        public int OriginalPosition { get; set; }
        public EnumActionToDoWithUncasted ActionToDoWithUncasted { get; set; }
        public List<Field> Fields { get; set; }

    }
}
