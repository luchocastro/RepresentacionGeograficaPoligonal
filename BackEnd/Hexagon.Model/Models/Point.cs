using System.Drawing;
namespace Hexagon.Model
{

    public class Point
    {
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }
        public override string ToString() => $"({X}, {Y})";

    }
}

