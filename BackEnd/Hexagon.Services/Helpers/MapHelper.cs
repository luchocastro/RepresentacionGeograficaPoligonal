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

namespace Hexagon.Services.Helpers
{
    public class MapHelper
    {

        public static List<Hex> HexMap (string PathJsonMap)
        {
            List<Hex> ret = new List<Hex>();
            
            var PolygonToTake = new List<Polygon>();
            var PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
            var Positions = new List<IPosition>();

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
                            foreach (var corPol in Polygon.Coordinates)
                            {
                                foreach (var Point in corPol.Coordinates)
                                {
                                    Positions.Add(Point);
                                }
                            }

                        }
                    }
                    if (typeof(Polygon) == featureItem.Geometry.GetType())
                    {

                        Polygon Polygon = featureItem.Geometry as Polygon;
                        foreach (var CordPolygon in Polygon.Coordinates
                            )
                        {

                            foreach (var Point in CordPolygon.Coordinates)
                            {
                                Positions.Add(Point);
                            }
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
                foreach (var item in Positions)
                {
                    var hexPosition = HexagonFunction.PixelToHexagon(new Layout(true, new System.Drawing.PointF (10f, 10f), new System.Drawing.PointF(0, 0)),
                                         new Model.Point(((float)item.Longitude ) , ((float)item.Latitude ) ));

                    if (!ret.Any(x => x == hexPosition))
                        ret.Add(hexPosition);
                }
            }
                
            return ret;
        }
    }
}
