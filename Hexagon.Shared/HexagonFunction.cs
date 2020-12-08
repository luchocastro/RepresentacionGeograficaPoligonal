using System;
using System.Collections.Generic;

/*namespace Hexagono
{
    public class HexagonFunction
    {

        public static readonly Hex[] HexagonDirections = new Hex[] { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };
        public static readonly Hex[] HexagonDiagonals = new Hex[] { new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2) };
        public static readonly int EVEN = 1;
        public static readonly int ODD = -1;
        public static readonly Orientation LayoutPointy = new Orientation((decimal)Math.Sqrt(3.0), (decimal)(Math.Sqrt (3.0) / 2.0), 0.0m, 3.0m / 2.0m, (decimal)Math.Sqrt(3.0) / 3.0m, -1.0m / 3.0m, 0.0m, 2.0m / 3.0m, 0.5m);
        public static readonly Orientation LayoutFlat = new Orientation(3.0m / 2.0m, 0.0m, (decimal)(Math.Sqrt(3.0) / 2.0), (decimal)Math.Sqrt(3.0), 2.0m / 3.0m, 0.0m, -1.0m / 3.0m, (decimal)(Math.Sqrt(3.0) / 3.0), 0.0m);


        public static Hex HexagonAdd(Hex A, Hex B)
        {
            return new Hex(A.Q + B.Q, A.R + B.R, A.S + B.S);
        }
        public static Hex HexagonSubtract(Hex A, Hex B)
        {
            return new Hex(A.Q - B.Q, A.R - B.R, A.S - B.S);
        }

        public static Hex HexagonScale(Hex A, decimal K)
        {
            return new Hex(A.Q * K, A.R * K, A.S * K);
        }

        public static Hex HexagonRotateLeft(Hex a)
        {
            return new Hex(-a.S, -a.Q, -a.R);
        }

        public static Hex HexagonRotateRight(Hex a)
        {
            return new Hex(-a.R, -a.S, -a.Q);
        }

        public static Hex HexagonDirection(int direction)
        {
            return HexagonDirections[direction];
        }

        public static Hex HexagonNeighbor(Hex hex, int direction)
        {
            return HexagonAdd(hex, HexagonDirection(direction));
        }
        public static Hex HexagonDiagonalNeighbor(Hex hex, int direction)
        {
            return HexagonAdd(hex, HexagonDiagonals[direction]);
        }
        public static decimal HexagonLength(Hex hex)
        {
            return (Math.Abs(hex.Q) + Math.Abs(hex.R) + Math.Abs(hex.S)) / 2m;
        }
        public static decimal HexagonDistance(Hex a, Hex b)
        {
            return HexagonLength(HexagonSubtract(a, b));
        }

        public static Hex HexagonRound(Hex h)
        {
            var qi = Math.Round(h.Q);
            var ri = Math.Round(h.R);
            var si = Math.Round(h.S);
            var q_diff = Math.Abs(qi - h.Q);
            var r_diff = Math.Abs(ri - h.R);
            var s_diff = Math.Abs(si - h.S);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                qi = -ri - si;
            }
            else if (r_diff > s_diff)
            {
                ri = -qi - si;
            }
            else
            {
                si = -qi - ri;
            }
            return new Hex(qi, ri, si);
        }

        public static Hex HexagonLerp(Hex a, Hex b, decimal t)
        {
            return new Hex(a.Q * (1.0m - t) + b.Q * t, a.R * (1.0m - t) + b.R * t, a.S * (1.0m - t) + b.S * t);
        }

        public static List<Hex> HexagonLinedraw(Hex a, Hex b)
        {
            var N = HexagonDistance(a, b);
            var a_nudge = new Hex(a.Q + 0.000001m, a.R + 0.000001m, a.S - 0.000002m);
            var b_nudge = new Hex(b.Q + 0.000001m, b.R + 0.000001m, b.S - 0.000002m);
            var results = new List<Hex>();
            var step = 1.0m / Math.Max(N, 1);
            for (var i = 0; i <= N; i++)
            {
                results.Add(HexagonRound(HexagonLerp(a_nudge, b_nudge, step * i)));
            }
            return results;
        }
    
        public static Coordinate QOffsetFromCube(decimal offset, Hex h)
        {
            var col = h.Q;
            var row = h.R + (h.Q + offset * ((int)h.Q & 1)) / 2;
            return new Coordinate(col, row);
        }

        public static Hex QOffsetToCube(decimal offset, Coordinate h)
        {
            var q = h.Col;
            var r = h.Row - (h.Col + offset * ((int)h.Col & 1)) / 2;
            var s = -q - r;
            return new Hex(q, r, s);
        }

        public static Coordinate ROffsetFromCube(decimal offset, Hex h)
        {
            var col = h.Q + (h.R + offset * ((int)h.R & 1)) / 2;
            var row = h.R;
            return new Coordinate(col, row);
        }

        public static Hex ROffsetToCube(decimal offset, Coordinate h)
        {
            var q = h.Col - (h.Row + offset * ((int) h.Row & 1)) / 2;
            var r = h.Row;
            var s = -q - r;
            return new Hex(q, r, s);
        }

        public static Coordinate QDoubledFromCube(Hex h)
        {
            var col = h.Q;
            var row = 2 * h.R + h.Q;
            return new Coordinate(col, row);
        }

        public static Hex QDoubledToCube(Coordinate h)
        {
            var q = h.Col;
            var r = (h.Row - h.Col) / 2;
            var s = -q - r;
            return new Hex(q, r, s);
        }

        public static Coordinate RDoubledFromCube(Hex h)
        {
            var col = 2 * h.Q + h.R;
            var row = h.R;
            return new Coordinate(col, row);
        }

        public static Hex RDoubledToCube(Coordinate h)
        {
            var q = (h.Col - h.Row) / 2;
            var r = h.Row;
            var s = -q - r;
            return new Hex(q, r, s);
        }
        public static Point HexagonToPixel(Layout layout, Hex h)
        {
            var M = layout.Orientation ;
            var size = layout.Size;
            var origin = layout.Origin;
            var x = (M.F0 * h.Q + M.F1 * h.R) * size.X ;
            var y = (M.F2 * h.Q + M.F3 * h.R) * size.Y;
            return new Point(x + origin.X, y + origin.Y );
        }

        public static Hex PixelToHexagon(Layout layout, Point p)
        {
            var M = layout.Orientation;
            var size = layout.Size;
            var origin = layout.Origin;
            var pt = new Point((p.X - origin.X) / size.X, (p.Y - origin.Y) / size.Y);
            var q = M.B0 * pt.X + M.B1 * pt.Y;
            var r = M.B2 * pt.X + M.B3 * pt.Y;
            return HexagonRound (new Hex(q, r, -q - r));
        }

        public static Point HexagonCornerOffset(Layout layout, decimal corner)
        {
            var M = layout.Orientation;
            var size = layout.Size;
            var angle = 2.0 * Math.PI * (double)(M.StartAngle - corner) / 6.0;
            return new Point(size.X * (decimal)Math.Cos(angle), size.Y * (decimal)Math.Sin(angle));
        }

        public static Point[] PolygonCorners(Layout layout, Hex h)
        {
            var corners = new Point[6];

            var center = HexagonToPixel (layout, h);
            for (var i = 0; i < 6; i++)
            {
                var offset = HexagonCornerOffset(layout, i);
                corners[i] = new Point(center.X + offset.X, center.Y + offset.Y);
            }
            return corners;
        }
    }
}
*/