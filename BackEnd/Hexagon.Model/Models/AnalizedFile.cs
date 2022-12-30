using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
     public struct AnalizedFile
    {
        public List<List<string>> Lines { get; set; }
        public List<Column> Columns { get; set; }
        public AnalizedFile(List<List<string>> Lines, List<Column> Columns)
        {
            this.Columns = Columns;
            this.Lines = Lines;
        }

    }
}
