namespace Hexagon.Shared
{
    
        public struct Point
        {
            public Point(decimal x, decimal y)
            {
                X = x;
                Y = y;
            }

            public decimal X { get; }
            public decimal Y { get; }

            public override string ToString() => $"({X}, {Y})";
        }
    }

