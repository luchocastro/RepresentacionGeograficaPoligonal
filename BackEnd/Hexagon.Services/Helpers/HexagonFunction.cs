using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hexagon.Model;
using Shared;
//using System.Drawing;
namespace Hexagon.Services.Helpers
{

    public class HexagonFunction
    {

        public static readonly Hex[] HexagonDirections = new Hex[] { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };
        public static readonly Hex[] HexagonDiagonals = new Hex[] { new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2) };
        public static readonly int EVEN = 1;
        public static readonly int ODD = -1;
        public static readonly Orientation LayoutPointy = new Orientation(MathF.Sqrt(3.0f), (MathF.Sqrt(3.0f) / 2.0f), 0.0f, 3.0f / 2.0f, (float)MathF.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);
        public static readonly Orientation LayoutFlat = new Orientation(3.0f / 2.0f, 0.0f, (MathF.Sqrt(3.0f) / 2.0f), (float)MathF.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, (float)(MathF.Sqrt(3.0f) / 3.0), 0.0f);
        public static readonly Layout LayoutBasic = new Layout(true, new System.Drawing.PointF(1, 1), new System.Drawing.PointF(0, 0), 600);

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
            return (MathF.Abs(hex.Q) + MathF.Abs(hex.R) + MathF.Abs(hex.S)) / 2f;
        }
        public static float HexagonDistance(Hex a, Hex b)
        {
            return HexagonLength(HexagonSubtract(a, b));
        }

        public static Hex HexagonRound(Hex h)
        {
            var qi = MathF.Round(h.Q);
            var ri = MathF.Round(h.R);
            var si = MathF.Round(h.S);
            var q_diff = MathF.Abs(qi - h.Q);
            var r_diff = MathF.Abs(ri - h.R);
            var s_diff = MathF.Abs(si - h.S);
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
            var step = 1.0f / MathF.Max(N, 1);
            for (var i = 0; i <= N; i++)
            {
                results.Add(HexagonRound(HexagonLerp(a_nudge, b_nudge, step * i)));
            }
            return results;
        }

        public static List<Hex> cube_linedraw(Hex a, Hex b)
        {
            var N = HexagonDistance(a, b);
            var results = new List<Hex>();
            for (int i = 0; i <= N; i++)
            {
                results.Add(HexagonRound(HexagonLerp(a, b, 1.0f / N * (float)i)));
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

        public static Hex PixelToHexagon(Point p)
        {
            
            var M = LayoutBasic.Orientation ;

            var pt = new Point((p.X), (p.Y));
            
            var q = M.B0 * pt.X  ;
            var r = M.B2 * pt.X + M.B3 * pt.Y;

            
            var hex = HexagonRound(new Hex(q, r, -q - r));
            hex.Corners = PolygonCorners(LayoutBasic, hex);
            return hex;
        }
        public static Hex PixelToHexagon(Layout layout, Point p)
        {
            var M = layout.Orientation;
            var size = layout.Size;
            var origin = layout.Origin;
            var pt = new Point((p.X ) / size.X, (p.Y - origin.Y) / size.Y);
            var q = (M.B0 * pt.X + M.B1 * pt.Y);
            var r = M.B2 * pt.X + M.B3 * pt.Y;
            return HexagonRound(new Hex(q, r, -q - r));
        }
        public static Point HexagonCornerOffset(Layout layout, float corner)
        {
            var M = layout.Orientation;
            var size = layout.Size;
            var angle = 2.0f * MathF.PI * (M.StartAngle - corner) / 6.0f;
            return new Point(size.X * (float)MathF.Cos(angle), size.Y * (float)MathF.Sin(angle));
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
        public static System.Drawing.PointF[] GetPoints(Hex Hexagono, Layout layout)
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

/*
 * void Coordenadas::
 
    CambioGeoUtm (int selector, char long_, char lat_, Coordenadas v, Coordenadas &t){
 
    const double pi = 3.14159265358979323846;
 
    double a, b;
 
    switch (selector) {
 
    case 1:
 
        a = 6378388.0, b = 6356911.946130;    //Hayford
     
                break;
 
    case 2:
 
        a = 6378137.0;  b = 6356752.3142;     // WGS 84
 
                break;
     
    }
 
// Sobre la geometria del elipsoide
// excentricidad = e1; segunda excentricidad = e2
 
    double e1, e2;
 
    e1 = sqrt (pow(a,2) - pow(b, 2)) / a;
    e2 = sqrt (pow(a,2) - pow(b, 2)) / b;
 
// Radio polar de curvatura y aplanamiento
// radio polar de curvatura = c; aplanamiento = alpha
 
    double c, alpha;
 
    c = (a*a)/b;
 
    alpha = (a -b) / a;
 
    double long_gd_rad, lat_gd_rad;
 
    if (long_ == 'E' || long_ == 'e') { 
 
        long_gd_rad = -(v.a[0] * pi)/180;
 
    }
 
        else {
 
        long_gd_rad = (v.a[0] * pi)/180;
         
        }
 
    lat_gd_rad = (v.a[1] * pi) / 180;
 
// Determinacion del Huso
 
    double huso_dec;
    int huso;
 
    huso_dec = v.a[0]/6 + 31;
 
    huso = int (huso_dec);
 
// Obtencion del meridiano central del uso = lambda0
 
    double lambda0, delta_lambda;
 
    lambda0 = (huso * 6.0 - 183.0)*(pi/180.0);
 
// Determinacion de la distancia angular que existe entre la longitud del punto (long_gd_rad) y
// el meridiano central del huso (lamda0)
 
    delta_lambda = long_gd_rad - lambda0;
 
// Ecuaciones de Coticchia-Surace para el Problema Directo (Paso de Geograficas a UTM)
// Calculo de Parametros
 
    double A, xi, eta, nu, zeta, A1, A2, J2, J4, J6, alpha2, beta, gamma, B_phi;
 
    A = cos(lat_gd_rad) * sin(delta_lambda);
 
    xi = 0.5 * log((1+A)/(1-A));
 
    eta = atan(tan(lat_gd_rad)/cos(delta_lambda)) - lat_gd_rad;
 
    nu = (c*0.9996)/sqrt((1 + e2*e2*cos(lat_gd_rad)*cos(lat_gd_rad)));
 
    zeta = (e2*e2/2)*(xi*xi)*(cos(lat_gd_rad)*cos(lat_gd_rad));
 
    A1 = sin(2.0*lat_gd_rad);
 
    A2 = A1 * (cos(lat_gd_rad)*cos(lat_gd_rad));
 
    J2 = lat_gd_rad + A1/2.0;
 
    J4 = (3*J2 + A2)/4;
 
    J6 = (5*J4 + A2 * (cos(lat_gd_rad)*cos(lat_gd_rad)))/3;
 
    alpha2 = (3.0/4.0)*(e2*e2);
 
    beta = (5.0/3.0)*(alpha2*alpha2);
 
    gamma = (35.0/27.0)*(pow(alpha2,3));
 
    B_phi = 0.9996 * c * (lat_gd_rad - alpha2 * J2 + beta * J4 - gamma * J6);
 
    t.a[0] = xi*nu*(1+zeta/3.0)+500000.00;
 
    t.a[1] = eta*nu*(1+zeta)+B_phi;
 
    if (lat_ == 'S' || lat_ == 's')
 
        t.a[1] += 10000000.0;
     
    t.a[2] = v.a[2];
 https://www.linz.govt.nz/guidance/geodetic-system/understanding-coordinate-conversions/projection-conversions/transverse-mercator-transformation-formulae
}*/