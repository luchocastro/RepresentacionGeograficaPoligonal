using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public struct HexgagonGrid
    {
        public HexgagonGrid(Hex[] HexagonMap)
        {
            this.HexagonMap = HexagonMap;
        }

         public Hex[] HexagonMap { get; }



    }
}
