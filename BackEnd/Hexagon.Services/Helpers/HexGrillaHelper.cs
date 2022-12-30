using Hexagon.Model;
using Hexagon.Services.CalcStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexagon.Services.Helpers
{

    public class HexGrillaHelper
    {
        private HexagonGrid _HexagonGrid;
        private List<EventPoint> _PuntosACalcular;
        public HexGrillaHelper(HexagonGrid HexagonGrid)
        {
            _HexagonGrid = HexagonGrid;
            _PuntosACalcular = HexagonGrid.PuntosACalcular;
        }
        private float MinX()
        {
            return _PuntosACalcular.Select(X => X.PositionInMeters.X).Min();
        }
        private float MaxX()
        {
            return _PuntosACalcular.Select(X => X.PositionInMeters.X).Max();
        }
        private float MinY()
        {
            return _PuntosACalcular.Select(X => X.PositionInMeters.Y).Min();
        }
        private float MaxY()
        {
            return _PuntosACalcular.Select(X => X.PositionInMeters.Y).Max();
        }
        private void CalcularHexagonos()
        {
            float MinY = this.MinY ();
            float MinX = this.MinX();

            float MaxY = this.MaxY();
            float MaxX = this.MaxX();
            List<Hex> Hexagonos = new List<Hex>();
            HexagonGrid Mapa = new HexagonGrid();
            float b = (float) (_HexagonGrid.Layout.Size.X * Math.Sin(30 * Math.PI / 180));
            // alert(b);
            
            var width = MaxX - MinX;
            var height = MaxY -MinY;
    var siguienteX = b ;
            var SiguienteY = 0f ;
            var lado = _HexagonGrid.Layout.Size.X;
            var a = lado * (float)Math.Cos(30 * Math.PI / 180);

            var paso = 0;
            while (siguienteX <= width  )
            {
                 paso = paso + 1;
                while (SiguienteY <= height)
                {
                    var hex = HexagonFunction.HexagonRound(HexagonFunction.PixelToHexagon(_HexagonGrid.Layout, new Point(siguienteX, SiguienteY)));
                    var corners = HexagonFunction.PolygonCorners(_HexagonGrid.Layout, hex);
                    float minx = corners.Select(X => X.X).Min();
                    float maxx = corners.Select(X => X.X).Max();
                    float miny = corners.Select(X => X.Y).Min();
                    float maxy = corners.Select(X => X.Y).Max();
                    var datos = _HexagonGrid.PuntosACalcular.Select(x => x).Where(x => x.PositionInMeters.X >= minx & x.PositionInMeters.X < maxx & x.PositionInMeters.Y >= miny & x.PositionInMeters.Y < maxy);
                    hex.Values = new List<EventPoint>();
                    
                    foreach (var item in datos)
                    {
                        var hexdato = HexagonFunction.PixelToHexagon(_HexagonGrid.Layout, new Point(item.PositionInMeters.X, item.PositionInMeters.Y));
                        if (hexdato == hex)
                        {
                            hex.Values.Add(item);
                        }
                    }


                    hex.Value = DoCalc.Do((object[]) hex.Values.Select(x => x.Values.ToArray()), _HexagonGrid.Function.Path, _HexagonGrid.Function.FullClassName, _HexagonGrid.Function.FunctionName);
                    Hexagonos.Add(hex);
                    SiguienteY = SiguienteY + 2f * a;
                }
                siguienteX = siguienteX + lado + b;

                if (paso % 2 == 1)
                {
                    SiguienteY = a ;
                }
                else
                {
                    SiguienteY = 0f;
                }
            }



        }
    }
}
