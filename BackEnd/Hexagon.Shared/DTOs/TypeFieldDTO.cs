using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class FieldTypeDTO
    {
        public List<FieldDataDTO> FieldData { get ; set; }

        public string FieldTypeName { get; set; }
        public string FieldTypePack { get; set; }
        public bool OwnIndexInData { get; set; }
    }
    public class FieldDataDTO
    {

        public string Mask { get; set; } = "";
        public string TypeName { get; set; }
        public int Order { get; set; }

    }

}
