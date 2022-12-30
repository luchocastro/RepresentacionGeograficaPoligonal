using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Hexagon.Model
{
    public struct EventPoint
    {
        
        public PointF PositionInMeters { get; set; }
        public PointF PositionInDegrees { get; set; }
        public DateTime EventTime { get; set; }
        public String Description { get; set; }
        public float  Value { get; set; }
        public List<object> Values { get; set; }
    }
}
