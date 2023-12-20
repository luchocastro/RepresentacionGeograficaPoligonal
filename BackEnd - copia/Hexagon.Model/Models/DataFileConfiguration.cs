using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public class DataFileConfiguration :Base
    {
        string _FileType;
        public string FileType { get { return _FileType; } set { _FileType = value; base.Name = value; }}
        public string DecimalSeparator { get; set; }
        public string DatetimeFormart { get; set; }
        public string TextDelimiter { get; set; }
        public Dictionary<string, object> FileProperties { get; set; }

    }
}
