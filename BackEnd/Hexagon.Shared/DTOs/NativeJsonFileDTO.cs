using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class NativeJsonFileDTO
    {
        public string Content { get; set; }
        public List<ColumnDTO> Columns { get; set; }
        
    }
}
