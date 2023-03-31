using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public struct  Field
    {
      
        public object Value { get; set; }
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get;}
        public long Index { get; set; }
    }
}
