using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public class DataFileConfiguration
    {
        public string FileType { get; set; }
        public Dictionary<string, object> FileProperties { get; set; }

    }
}
