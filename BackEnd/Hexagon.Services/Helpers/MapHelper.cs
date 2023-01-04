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
namespace Hexagon.Services.Helpers
{
    public class MapHelper
    {

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
                
                ///HAY QUE VER CÒMO HACERLO PERO LA DISTANCIA PROMEDIO ENTRE LOS HEXA TIENE QUE SER EL TAMAÑO DE LOS LADOS HEXAGAONOS   
                var Max = (rangeX > rangeY ? rangeX : rangeY);
                var Unidad = 3200f / Max;
                var hexPerLine = rangeX * Unidad;
                //layout = new Layout(layout.Flat , new System.Drawing.PointF(Max/hexPerLine  , Max / hexPerLine), layout.Origin,layout.HexPerLine);
                layout = new Layout(layout.Flat , new System.Drawing.PointF(Max/(float)layout.HexPerLine , Max / (float)layout.HexPerLine), layout.Origin,layout.HexPerLine);
                //foreach (var item in Positions)
                //{
                //    var hexPosition = HexagonFunction.PixelToHexagon(layout ,
                //                         new Model.Point(((float)item.Longitude ) , ((float)item.Latitude ) ));


                //    //if (!ret.Any(x => x == HexagonFunction.HexagonRound( hexPosition)))
                //        ret.Add((hexPosition));
                //}
                foreach (var item in PoliygonList)
                {


                    for (int i = 0; i < item.Count - 1; i++)
                    {
                            var hexPosition1 = HexagonFunction.PixelToHexagon(layout ,
                                                 new Model.Point((item[i].X ) , (item[i].Y) ));
                        var hexPosition2 = HexagonFunction.PixelToHexagon(layout,
                                                 new Model.Point((item[i+1].X), (item[i + 1].Y)));
                        var linea = HexagonFunction.HexagonLinedraw(hexPosition1, hexPosition2);
                        ret.AddRange(linea);
                    }
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
