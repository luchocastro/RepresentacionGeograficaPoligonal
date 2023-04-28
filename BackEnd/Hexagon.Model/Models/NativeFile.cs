using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class NativeFile : Base
    {
        string _FileName;
        string _PathFile;
        public string ContentID { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();
        public string PathFile { get { return _PathFile; }  set { _PathFile = value;base.Path = value; } }
        public DataFileConfiguration DataFileConfiguration { get; set; } = null;
        
        public bool IsPolygon { get; set; } = false;

        public string FileName { get { return _FileName; } set { _FileName = value; base.Name = value; } }

    }
}
