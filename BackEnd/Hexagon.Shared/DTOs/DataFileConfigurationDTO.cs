using Hexagon.Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class DataFileConfigurationDTO :BaseDto
    {
        public string FileType { get; set; }
        public Dictionary<string, object> FileProperties { get; set; }
        public string DecimalSeparator { get; set; }
        public string DatetimeFormart { get; set; }
        public string TextDelimiter { get; set; }
        
    }
}
