using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class NativeFileDTO :Base.BaseDto
    {
        public List<LineDTO> Content { get; set; }
        public List<string > Columns { get; set; }
        public string PathFile { get; set; }
        public DataFileConfigurationDTO DataFileConfigurationDTO { get; set; }

        public string FileName { get; set; }
        public string ColumnXY { get; set; }
        public bool IsPolygon { get; set; }
    }
}
