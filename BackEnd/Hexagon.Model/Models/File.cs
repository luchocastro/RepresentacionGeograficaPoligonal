using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class HexFile : Equatable<HexFile>
    {

        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Path { get; set; }

    }
}
