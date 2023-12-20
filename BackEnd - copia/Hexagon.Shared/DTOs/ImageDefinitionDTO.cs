using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    
        public struct ImageDefinitionDTO
        {
            public float OriginalMinX { get; }
            public float OriginalMinY { get; }
            public float OriginalMaxX { get; }
            public float OriginalMaxY { get; }

            public float OriginalRangeX { get; }
            public float OriginalRangeY { get; }
            public float HexagonSize { get; set; }
            public float ProportationToScale { get; }
            public float TransformedWidth { get; }
            public float TransformedHeigth { get; }

            public float MaxProportion { get; }
            public LayoutDto Layout { get; }
            public PointDTO Scale { get; set; }
        }
}
