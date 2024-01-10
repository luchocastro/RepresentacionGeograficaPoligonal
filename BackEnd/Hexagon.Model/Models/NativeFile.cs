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
        string _DirectoryOfColumns;
        public string ContentID { get; set; }
        public List<String> Columns { get; set; } = new List<String>();
        public string PathFile { get { return _PathFile; }  set { _PathFile = value; } }
        public DataFileConfiguration DataFileConfiguration { get; set; }  
        public string DirectoryOfColumns { get { return _DirectoryOfColumns; } set { _DirectoryOfColumns = value; } }
        public bool IsPolygon { get; set; } = false;

        public string ColumnXY { get; set; }
        public string FileName { get { return _FileName; } set { _FileName = value;  } }
        //public override string Name { get => _FileName; set => _FileName = value; }
        
    }
}
