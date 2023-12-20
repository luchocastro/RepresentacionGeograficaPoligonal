using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Hexagon.Shared.DTOs
{
    public class FieldDTO
    {
        
        public object Value { get; set; }
            [JsonIgnore]
            public List<EnumAlowedDataTypeDTO> DataTypeFinded { get; }
            public long Index { get; set; }
        
    }
}
