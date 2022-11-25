using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public struct  Field
    {
        public Field(object Value, List<EnumAlowedDataType> DataTypeFinded, int Line)
        {
            this.Value = Value;
            this.DataTypeFinded = DataTypeFinded;
            this.Index = Line;
        }
        public object Value { get; }
        public List<EnumAlowedDataType> DataTypeFinded { get;}
        public int Index { get; set; }
    }
}
