using System;
using System.Collections.Generic;
using System.Text;


namespace Hexagon.Model
{
    public class Column
    {
        public Column(string Name, int OriginalPosition, EnumActionToDoWithUncasted ActionToDoWithUncasted)
        {
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
        }
        public List<EnumAlowedDataType> DataTypeFinded { get; set; }
        public EnumAlowedDataType DataTypeSelected { get; set; }
        public string Name { get; }
        public int OriginalPosition { get; }
        public EnumActionToDoWithUncasted ActionToDoWithUncasted { get; }
        public List<Field> Fields { get; set; }
    }
}
