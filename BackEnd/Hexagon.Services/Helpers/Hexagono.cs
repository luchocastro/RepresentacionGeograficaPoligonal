using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hexagon.Model;
using Shared;
namespace Hexagon.Services.Helpers
{
    
        public class HexagonFunction
        {

            public static readonly Hex[] HexagonDirections = new Hex[] { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };
            public static readonly Hex[] HexagonDiagonals = new Hex[] { new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2) };
            public static readonly int EVEN = 1;
            public static readonly int ODD = -1;
            public static readonly Orientation LayoutPointy = new Orientation((float)Math.Sqrt(3.0), (float)(Math.Sqrt(3.0) / 2.0), 0.0f, 3.0f/ 2.0f, (float)Math.Sqrt(3.0) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);
            public static readonly Orientation LayoutFlat = new Orientation(3.0f/ 2.0f, 0.0f, (float)(Math.Sqrt(3.0) / 2.0), (float)Math.Sqrt(3.0), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, (float)(Math.Sqrt(3.0) / 3.0), 0.0f);


            public static Hex HexagonAdd(Hex A, Hex B)
            {
                return new Hex(A.Q + B.Q, A.R + B.R, A.S + B.S);
            }
            public static Hex HexagonSubtract(Hex A, Hex B)
            {
                return new Hex(A.Q - B.Q, A.R - B.R, A.S - B.S);
            }

            public static Hex HexagonScale(Hex A, float K)
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
            public static float HexagonLength(Hex hex)
            {
                return (Math.Abs(hex.Q) + Math.Abs(hex.R) + Math.Abs(hex.S)) / 2f;
            }
            public static float HexagonDistance(Hex a, Hex b)
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
                return new Hex((float)qi, (float)ri, (float)si);
            }

            public static Hex HexagonLerp(Hex a, Hex b, float t)
            {
                return new Hex(a.Q * (1.0f - t) + b.Q * t, a.R * (1.0f - t) + b.R * t, a.S * (1.0f - t) + b.S * t);
            }

            public static List<Hex> HexagonLinedraw(Hex a, Hex b)
            {
                var N = HexagonDistance(a, b);
                var a_nudge = new Hex(a.Q + 0.000001f, a.R + 0.000001f, a.S - 0.000002f);
                var b_nudge = new Hex(b.Q + 0.000001f, b.R + 0.000001f, b.S - 0.000002f);
                var results = new List<Hex>();
                var step = 1.0f / Math.Max(N, 1);
                for (var i = 0; i <= N; i++)
                {
                    results.Add(HexagonRound(HexagonLerp(a_nudge, b_nudge, step * i)));
                }
                return results;
            }

            public static Coordinate QOffsetFromCube(float offset, Hex h)
            {
                var col = h.Q;
                var row = h.R + (h.Q + offset * ((int)h.Q & 1)) / 2;
                return new Coordinate(col, row);
            }

            public static Hex QOffsetToCube(float offset, Coordinate h)
            {
                var q = h.Col;
                var r = h.Row - (h.Col + offset * ((int)h.Col & 1)) / 2;
                var s = -q - r;
                return new Hex(q, r, s);
            }

            public static Coordinate ROffsetFromCube(float offset, Hex h)
            {
                var col = h.Q + (h.R + offset * ((int)h.R & 1)) / 2;
                var row = h.R;
                return new Coordinate(col, row);
            }

            public static Hex ROffsetToCube(float offset, Coordinate h)
            {
                var q = h.Col - (h.Row + offset * ((int)h.Row & 1)) / 2;
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
                var M = layout.Orientation;
                var size = layout.Size;
                var origin = layout.Origin;
                var x = (M.F0 * h.Q + M.F1 * h.R) * size.X;
                var y = (M.F2 * h.Q + M.F3 * h.R) * size.Y;
                return new Point(x + origin.X, y + origin.Y);
            }
        public static float scaleLinear(float unscaledNum, float minAllowed, float maxAllowed, float min, float max)
            {
            return (maxAllowed - minAllowed) * (unscaledNum - min) / (max - min) + minAllowed; ;
        } 
        public static Point Scale ( Point PointToScale, float MaxX, float MaxY, float MinX, float MinY, float Width, float Heigth, float Orwi, float Orhe)
        {
            
            
            
            var prop = (MaxY - MinY) / (MaxX - MinX);
            //console.log('width:' + width);
            var height = Width * prop; //canvas.attr('height');
            //console.log("prop " + prop);
            var scaleX =  HexagonFunction.scaleLinear(PointToScale.X , 0, Orhe, MinX, MinY) ;
             
            return new Point (0f,0f) ;
        }
            public static Hex PixelToHexagon(Layout layout, Point p)
            {
                var M = layout.Orientation;
                var size = layout.Size;
                var origin = layout.Origin;
                var pt = new Point((p.X - origin.X) / size.X, (p.Y - origin.Y) / size.Y);
                var q = M.B0 * pt.X + M.B1 * pt.Y;
                var r = M.B2 * pt.X + M.B3 * pt.Y;
                return HexagonRound(new Hex(q, r, -q - r));
            }

            public static Point HexagonCornerOffset(Layout layout, float corner)
            {
                var M = layout.Orientation;
                var size = layout.Size;
                var angle = 2.0 * Math.PI * (M.StartAngle - corner) / 6.0;
                return new Point(size.X * (float)Math.Cos(angle), size.Y * (float)Math.Sin(angle));
            }

            public static Point[] PolygonCorners(Layout layout, Hex h)
            {
                var corners = new Point[6];

                var center = HexagonToPixel(layout, h);
                for (var i = 0; i < 6; i++)
                {
                    var offset = HexagonCornerOffset(layout, i);
                    corners[i] = new Point(center.X + offset.X, center.Y + offset.Y);
                }
                return corners;
            }
        public static System.Drawing.PointF[] GetPoints(Hex Hexagono , Layout layout )
        {
            //{
            //     System.Drawing.Point[] CornersToDraw = new System.Drawing.Point[6];
            //    Point[] PolygonCorners = HexagonFunction.PolygonCorners(layout, Hexagono);
            //    //PointsToPaint.Add(Hexagono.)
            //    for (int i = 0; i < 6; i++)
            //    {
            //        CornersToDraw[i]= new System.Drawing.Point ( (int)PolygonCorners [i].X, (int)PolygonCorners[i].Y);
            //    }
            //    return CornersToDraw;

            //}
            var ret = HexagonFunction.PolygonCorners(layout, Hexagono).Select(x => new System.Drawing.PointF(x.X, x.Y));
            return ret.ToArray();
        }
        public static Point[] Draw(Point center, Layout layout)
        {
            return null;
        }



    }
}

