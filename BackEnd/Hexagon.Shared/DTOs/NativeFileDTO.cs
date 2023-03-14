using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class NativeFileDTO
    {
        public List<LineDTO> Content { get; set; }
        public List<ColumnDTO> Columns { get; set; }
        public string PathFile { get; set; }
        public DataFileConfigurationDTO DataFileConfigurationDTO { get; set; }
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string FileName { get; set; }
    }
}
