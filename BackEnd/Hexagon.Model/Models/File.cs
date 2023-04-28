using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class HexFile : Equatable<HexFile>
    {
        string _OriginalFileName;
        public string FileName { get; set; }
        public string OriginalFileName { get{return _OriginalFileName; } set { _OriginalFileName= value; base.Name = value; } }

        public string TypeFile { get; set; }

    }
}
