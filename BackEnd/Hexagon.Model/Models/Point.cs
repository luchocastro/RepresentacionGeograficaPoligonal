using Hexagon.Model.Helper;
using Hexagon.Model.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hexagon.Model
{
    public class Point : Package<Point>
    {
        public Point()
        {

        }
        public Point(float x, float y)
        {
            X = x;
            Y = y;

        }
        public Point(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }
        public float X { get; }

        public float Y { get; }
        public override string ToString()
        {
            return "[" + this.XText + "," + this.YText + "]";
                }

        public Point FromString(string Point)
        {
            var despoint =  Point.Replace("[","").Replace("]","").Split(",");
            return new Point(despoint[0], despoint[1]);
        }
        public Point(string x, string y)
        {
            X = Utils.FloatFromString(x);
            Y = Utils.FloatFromString(y);
        }
        [JsonIgnore]
        [ModelSaveAtributes(InPackage = true, PropertyOrder = 0)]
        public string XText { get { return Utils.FloatFromFloat(this.X); } }
        [JsonIgnore]
        [ModelSaveAtributes(InPackage = true, PropertyOrder = 1)]
        public string YText { get { return Utils.FloatFromFloat(this.Y); } }

    }
}

