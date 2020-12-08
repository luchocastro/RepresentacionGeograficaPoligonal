using System;
using System.Collections.Generic;
using System.Text;
using static Hexagon.Model.EnumActionToDoWithUncasted;

namespace Hexagon.Model
{
    public class Column
    {
        public Column(string Name, int OriginalPosition, ActionToDoWithUncasted ActionToDoWithUncasted)
        {
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
        }
        public List<EnumDataType> DataTypeFinded { get; set; }
        public EnumDataType DataTypeSelected { get; set; }
        public string Name { get; }
        public int OriginalPosition { get; }
        public ActionToDoWithUncasted ActionToDoWithUncasted { get; }
    }
}
