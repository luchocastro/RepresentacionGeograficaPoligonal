using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class MapDefinition : Equatable<MapDefinition>
    { 
        public Column ColumnForX { get; set; } 
        public Column ColumnForY { get; set; }
        public Column ColumnForMapGroup { get; set; }
        public List<string> ColumnsToDiscard { get; set; }
        public List<Column> DataTypedColumns { get; set; }


    }
}
