using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hexagon.Model.Models
{
    public class PointDistance
    {
        public Point Point { get; set; }
        public float UnitsFromMaxXMaxY { get; set; }
        public float UnitsFromMaxXMinY { get; set; }
        public float UnitsFromMinXMaxY { get; set; }
        public float UnitsFromMinXMinY { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize<PointDistance> (this);

        }
        public static PointDistance Get(String PointDistanceFromString )
        {
            return JsonSerializer.Deserialize<PointDistance>(PointDistanceFromString);
        }
    }
}
