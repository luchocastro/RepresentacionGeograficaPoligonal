using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Hexagon.Model
{
    public class Column :Base
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
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get; set; } = (new EnumAlowedDataType[] { EnumAlowedDataType.Character }).ToList();
        [JsonIgnore]
        public EnumAlowedDataType DataTypeSelected { get; set; }
        public string Name { get; set; }
        public int OriginalPosition { get; set; }
        [JsonIgnore]
        public EnumActionToDoWithUncasted ActionToDoWithUncasted { get; set; }
        public List<Field> Fields { get; set; }
        public string PathContentID { get; set; }

    }
}
