using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class DataFileConfigurationDTO
    {
        public string FileType { get; set; }
        public Dictionary<string, object> FileProperties { get; set; }
    }
}
