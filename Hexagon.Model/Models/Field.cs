using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public struct  Field
    {
        public Field(object Value, List<EnumDataType.AlowedDataType> DataTypeFinded)
        {
            this.Value = Value;
            this.DataTypeFinded = DataTypeFinded;
        }
        public object Value { get; }
        public List<EnumDataType.AlowedDataType> DataTypeFinded { get;}
    }
}
