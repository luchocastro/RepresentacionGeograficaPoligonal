using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Converters;
using GeoJSON.Net;
using Newtonsoft.Json;
using System.Linq;
using CliWrap;
using CliWrap.Buffered;
using System.Drawing;

namespace Hexagon.Services.Helpers
{
    public class MapHelper
    {
        public static void PaintHexInsidePolygon(List<PointF> Points, ref List <Hex> Hexs,  Layout Layout)
        {
            if (Points.Count == 0) return;
            var PolyX = Points.Select(X => (float)X.X).ToArray();
            var PolyY = Points.Select(X => (float)X.Y).ToArray();
            var polyCorners = Points.Count ;
            var IMAGE_TOP = Points.Min(x => x.Y);
            var IMAGE_BOT = Points.Max(x => x.Y);
            var IMAGE_RIGHT = Points.Max(x => x.X);
            var IMAGE_LEFT = Points.Min(x => x.X);

            for (float x = IMAGE_LEFT; x <= IMAGE_RIGHT; x = x + MathF.Sqrt(3) * Layout.Size.X)
                for (float y = IMAGE_TOP; y <= IMAGE_BOT; y = y + MathF.Sqrt (3) * (Layout.Size.Y / 2) )
            {
                    if (pointInPolygon(polyCorners, PolyX, PolyY, new PointF(x, y)))
                        {
                        var hex = HexagonFunction.PixelToHexagon(Layout, new Hexagon.Model.Point(x, y));
                        hex.RGBColor = new int[] { Color.Pink.R, Color.Pink.R, Color.Pink.B };
                        Hexs.Add(hex);
                    }
                }
        }
        static bool pointInPolygon(int polyCorners, float[] polyX, float[] polyY, PointF ToBeTested)
        {
            float x = ToBeTested.X, y = ToBeTested.Y;
            int i, j = polyCorners - 1;
            bool oddNodes = false;

            for (i = 0; i < polyCorners; i++)
            {
                if (polyY[i] < y && polyY[j] >= y
                || polyY[j] < y && polyY[i] >= y)
                {
                    if (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }

            return oddNodes;

        }

    
public static List<Hex> HexMapGeoJSon (string PathJsonMap, ref Layout layout)
        {
            List<Hex> ret = new List<Hex>();

            var PolygonToTake = new List<Polygon>();
            var PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
            var Positions = new List<IPosition>();
            var PoliygonList = new List<List<Model.Point>> ();
            using (StreamReader file = File.OpenText(PathJsonMap))
            {
                var mapa = file.ReadToEnd();
                var feature = JsonConvert.DeserializeObject<FeatureCollection>(mapa);

                foreach (var featureItem in feature.Features)
                {
                    if (typeof(MultiPolygon) == featureItem.Geometry.GetType())
                    {
                        MultiPolygon multiPolygon = featureItem.Geometry as MultiPolygon;
                        
                        foreach (var Polygon in multiPolygon.Coordinates)
                        {

                            var Figure = new List<Model.Point>();
                            foreach (var corPol in Polygon.Coordinates)
                            {

                                foreach (var Point in corPol.Coordinates)
                                { 
                                    Figure.Add(new Model.Point((float)Point.Longitude, (float)Point.Latitude));
                                    Positions.Add(Point);
                                }
                            }
                            PoliygonList.Add(Figure);
                        }
                    }
                    if (typeof(Polygon) == featureItem.Geometry.GetType())
                    {

                        Polygon Polygon = featureItem.Geometry as Polygon;
                        foreach (var CordPolygon in Polygon.Coordinates
                            )
                        {
                            var Figure = new List<Model.Point>();
                            foreach (var Point in CordPolygon.Coordinates)
                            {
                                Figure.Add(new Model.Point((float)Point.Longitude, (float)Point.Latitude));

                                Positions.Add(Point );
                            }
                            PoliygonList.Add(Figure);
                        }


                    }
                    //foreach (int i = 0; if++ featureItem. .  )
                    //{
                    //    MultiPolygon multiPolygon = featureItem.Geometry as MultiPolygon;
                    //    foreach (var multiPolygonCoordinates in multiPolygon.Coordinates)
                    //    {
                    //        foreach (var item1 in  multiPolygonCoordinates.Coordinates)
                    //        {
                    //            foreach (var item2 in item1.Coordinates)
                    //            {
                    //                GeoJSON.Net.Converters.PositionConverter PositionConverter = new PositionConverter();
                    //                //Here I want to add value 0 altitude coordinate
                    //                Positions.Add(item2);
                    //             }
                    //        }
                    //    }
                    //}
                }
                var minX = (float) Positions.Min(x => x.Longitude);
                var maxX = (float)Positions.Max(x => x.Longitude);
                var QPositions = (float)Positions.Count();
                var minY = (float)Positions.Min(x => x.Latitude);
                var maxY = (float)Positions.Max(x => x.Latitude);
                var rangeX = (maxX - minX);
                var rangeY = maxY - minY;
                var prop = 0f;
                
                var width = 0f; //Convert.ToInt32 ( HexagonFunction.scaleLinear (1600f, 0,1600f, 0f , minX,maxY));
                var heigth = 0f; // Convert.ToInt32(HexagonFunction.scaleLinear(maxY, maxY - minY, 0, 0, 1600)); ;
                var mayor = 0f;

                ///HAY QUE VER CÒMO HACERLO PERO LA DISTANCIA PROMEDIO ENTRE LOS HEXA TIENE QUE SER EL TAMAÑO DE LOS LADOS HEXAGAONOS   
                var Max = (rangeX > rangeY ? rangeX : rangeY);

                if (maxX - minX > maxY - minY)
                {
                    width = layout.MaxPictureSizeX;
                    prop = width / (maxX - minX);
                    heigth = ((maxY - minY)) * prop;
                    mayor = width;

                }
                else
                {
                    heigth = layout.MaxPictureSizeY;
                    prop = heigth / (maxY - minY);
                    width = (maxX - minX) * prop;
                    mayor = heigth;
                }

                var Unidad = MathF.Round(mayor / layout.HexPerLine);
                layout = new Layout(layout.Flat , new System.Drawing.PointF(Unidad, Unidad), new PointF (width/2f, heigth/2f),layout.HexPerLine,width,heigth);
                //layout = new Layout(layout.Flat , new System.Drawing.PointF(Max/(float)layout.HexPerLine , Max / (float)layout.HexPerLine), layout.Origin,layout.HexPerLine);
                //foreach (var item in Positions)
                //{
                //    var hexPosition = HexagonFunction.PixelToHexagon(layout ,
                //                         new Model.Point(((float)item.Longitude ) , ((float)item.Latitude ) ));


                //    //if (!ret.Any(x => x == HexagonFunction.HexagonRound( hexPosition)))
                //        ret.Add((hexPosition));
                //}
                foreach (var item in PoliygonList)
                {
                    Hex HexAnterior = new Hex();

                    var poligon = new List<Hex> ();
                    var linea = new List<Hex>();
                    var PointsHexCornes = new List<PointF>();
                    for (int i = 0; i < item.Count ; i++)
                    {
                        var X =  (item[i].X - minX) * prop ;
                        var Y = (maxY - minY - (item[i].Y - minY)) * prop ;
                        var hexPosition1 = HexagonFunction.PixelToHexagon(layout,
                                                 new Model.Point(X,Y));
                        hexPosition1.RGBColor =  new int[] { Color.Blue.R, Color.Blue.R, Color.Blue.B };
                        ret.Add(hexPosition1);
                        PointsHexCornes.Add(new PointF(X, Y));  
                        if (i != 0 && hexPosition1 != HexAnterior)
                        {
                            linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);
                            poligon.AddRange(linea);
                            for (int x=0;x<linea.Count();x++)
                            {
                                var hex = linea[x];
                                hex.RGBColor = new int[] { Color.Blue.R, Color.Blue.R, Color.Blue.B };

                            }
                            ret.AddRange(linea);

                        }
                        HexAnterior = HexagonFunction.PixelToHexagon(layout,
                                                 new Model.Point(X,Y));
                    }
                    //PaintHexInsidePolygon(PointsHexCornes, ref ret, layout); 
                }
            }
    //        var pathToPy =  Cli.Wrap("which").WithArguments(new[] { "python3" }).ExecuteBufferedAsync();
    //        var resOutput = pathToPy.GetAwaiter();
    //        var result =  Cli.Wrap("/python/bin/python")
    //.WithArguments(new[] { "my_python_script.py", "arg1", "arg2" })
    //.WithWorkingDirectory("/app/python_scripts")
    //.ExecuteBufferedAsync();

    //        var output = result.ConfigureAwait(false);
    //        var res = output.GetAwaiter();
                return ret;
        }
        static float standardDeviation(IEnumerable<float> sequence)
        {
            float result = 0;

            if (sequence.Any())
            {
                float average = sequence.Average();
                float sum = sequence.Sum(d => MathF.Pow(d - average, 2));
                result = MathF.Sqrt((sum) / sequence.Count());
            }
            return result;
        }
    }

}
