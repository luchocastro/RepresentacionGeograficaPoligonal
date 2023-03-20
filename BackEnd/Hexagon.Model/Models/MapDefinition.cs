using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class MapDefinition : Equatable<MapDefinition>
    { 
        public string ColumnNameForX { get; set; }
        public string ColumnNameForY { get; set; }
        public string ColumnForMapGroup { get; set; }

        

    }
}
