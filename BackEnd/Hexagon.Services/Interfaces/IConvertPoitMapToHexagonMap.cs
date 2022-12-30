using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    interface IConvertPoitMapToHexagonMap

    { 
        public Hex[] Hexgrid(Point[] Puntos);
    }
}
