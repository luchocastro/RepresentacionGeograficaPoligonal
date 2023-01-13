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
using Hexagon.Model.Models;

namespace Hexagon.Services.Helpers
{
    public class MapHelper
    {
        public static void PaintHexInsidePolygon(List<PointF> Points, ref List<Hex> Hexs, Layout Layout)
        {
            if (Points.Count == 0) return;
            var PolyX = Points.Select(X => (float)X.X).ToArray();
            var PolyY = Points.Select(X => (float)X.Y).ToArray();
            var polyCorners = Points.Count;
            var IMAGE_TOP = Points.Min(x => x.Y);
            var IMAGE_BOT = Points.Max(x => x.Y);
            var IMAGE_RIGHT = Points.Max(x => x.X);
            var IMAGE_LEFT = Points.Min(x => x.X);

            for (float x = IMAGE_LEFT; x <= IMAGE_RIGHT; x = x + MathF.Sqrt(3) * Layout.Size.X)
                for (float y = IMAGE_TOP; y <= IMAGE_BOT; y = y + MathF.Sqrt(3) * (Layout.Size.Y / 2))
                {
                    if (pointInPolygon(polyCorners, PolyX, PolyY, new PointF(x, y)))
                        Hexs.Add(HexagonFunction.PixelToHexagon(Layout, new Hexagon.Model.Point(x, y)));
                }
        }
        public static void PaintHexInsidePolygon2(List<PointF> Points, List<Hex> Poligon, ref List<Hex> Hexs, Layout Layout)
        {
            if (Points.Count == 0) return;
            var PolyX = Points.Select(X => (float)X.X).ToArray();
            var PolyY = Points.Select(X => (float)X.Y).ToArray();
            var polyCorners = Points.Count;
            var IMAGE_TOP = Points.Min(x => x.Y);
            var IMAGE_BOT = Points.Max(x => x.Y);
            var IMAGE_RIGHT = Points.Max(x => x.X);
            var IMAGE_LEFT = Points.Min(x => x.X);

            for (float y = IMAGE_TOP; y <= IMAGE_BOT; y = y + MathF.Sqrt(3) * Layout.Size.Y)
            {
                for (float x = IMAGE_LEFT; x <= IMAGE_RIGHT; x = x + MathF.Sqrt(3) * Layout.Size.X)
                {
                    var hexToTest = HexagonFunction.PixelToHexagon(Layout, new Model.Point(x, y));
                    var countToLeft = Poligon.Where(X => X.R == hexToTest.R && X.S > hexToTest.S).Count();
                    var countToRight = Poligon.Where(X => X.R == hexToTest.R && X.S < hexToTest.S).Count();
                    if (countToRight % 2f == 1f && countToLeft % 2f == 1)
                        Hexs.Add(HexagonFunction.PixelToHexagon(Layout, new Hexagon.Model.Point(x, y)));
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


        //public static List<Hex> HexMapGeoJSon(string PathJsonMap, ref Layout layout)
        //{
        //    List<Hex> ret = new List<Hex>();

        //    var PolygonToTake = new List<Polygon>();
        //    var PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
        //    var Positions = new List<IPosition>();
        //    var PoliygonList = new List<List<Model.Point>>();
        //    using (StreamReader file = File.OpenText(PathJsonMap))
        //    {
        //        var mapa = file.ReadToEnd();
        //        var feature = JsonConvert.DeserializeObject<FeatureCollection>(mapa);

        //        foreach (var featureItem in feature.Features)
        //        {
        //            if (typeof(MultiPolygon) == featureItem.Geometry.GetType())
        //            {
        //                MultiPolygon multiPolygon = featureItem.Geometry as MultiPolygon;

        //                foreach (var Polygon in multiPolygon.Coordinates)
        //                {

        //                    var Figure = new List<Model.Point>();
        //                    foreach (var corPol in Polygon.Coordinates)
        //                    {

        //                        foreach (var Point in corPol.Coordinates)
        //                        {
        //                            Figure.Add(new Model.Point((float)Point.Longitude, (float)Point.Latitude));
        //                            Positions.Add(Point);
        //                        }
        //                    }
        //                    PoliygonList.Add(Figure);
        //                }
        //            }
        //            if (typeof(Polygon) == featureItem.Geometry.GetType())
        //            {

        //                Polygon Polygon = featureItem.Geometry as Polygon;
        //                foreach (var CordPolygon in Polygon.Coordinates
        //                    )
        //                {
        //                    var Figure = new List<Model.Point>();
        //                    foreach (var Point in CordPolygon.Coordinates)
        //                    {
        //                        Figure.Add(new Model.Point((float)Point.Longitude, (float)Point.Latitude));

        //                        Positions.Add(Point);
        //                    }
        //                    PoliygonList.Add(Figure);
        //                }


        //            }

        //        }
        //        var minX = (float)Positions.Min(x => x.Longitude);
        //        var maxX = (float)Positions.Max(x => x.Longitude);
        //        var QPositions = (float)Positions.Count();
        //        var minY = (float)Positions.Min(x => x.Latitude);
        //        var maxY = (float)Positions.Max(x => x.Latitude);
        //        var rangeX = (maxX - minX);
        //        var rangeY = maxY - minY;
        //        var prop = 0f;

        //        var width = 0f;
        //        var heigth = 0f;
        //        var mayor = 0f;
        //        var Unidad = 2f;
        //        ///HAY QUE VER CÒMO HACERLO PERO LA DISTANCIA PROMEDIO ENTRE LOS HEXA TIENE QUE SER EL TAMAÑO DE LOS LADOS HEXAGAONOS   
        //        var Max = (rangeX > rangeY ? rangeX : rangeY);

        //        if (maxX - minX > maxY - minY)
        //        {
        //            if (MathF.Floor(layout.MaxPictureSizeX / layout.HexPerLine) > Unidad)
        //            {
        //                Unidad = MathF.Floor(layout.MaxPictureSizeX / layout.HexPerLine);
        //            }
        //            layout.MaxPictureSizeX = layout.HexPerLine * Unidad;
        //            width = layout.MaxPictureSizeX;
        //            prop = MathF.Floor(width / (maxX - minX));
        //            heigth = MathF.Ceiling(((maxY - minY)) * prop);
        //            mayor = width;
        //            layout.MaxPictureSizeY = heigth;

        //        }
        //        else
        //        {
        //            if (MathF.Floor(layout.MaxPictureSizeY / layout.HexPerLine) > Unidad)
        //            {
        //                Unidad = MathF.Floor(layout.MaxPictureSizeY / layout.HexPerLine);
        //            }
        //            layout.MaxPictureSizeY = layout.HexPerLine * Unidad;
        //            heigth = layout.MaxPictureSizeY;
        //            prop = MathF.Floor(heigth / (maxY - minY));
        //            width = MathF.Ceiling((maxX - minX) * prop);
        //            mayor = heigth;
        //            layout.MaxPictureSizeX = width;
        //        }

        //        layout = new Layout(layout.Flat, new System.Drawing.PointF(Unidad, Unidad), new PointF(width / 2f, heigth / 2f), layout.HexPerLine, width, heigth, layout.FillPolygon);

        //        foreach (var item in PoliygonList)
        //        {
        //            Hex HexAnterior = new Hex();

        //            var poligon = new List<Hex>();
        //            var linea = new List<Hex>();
        //            var PointsHexCornes = new List<PointF>();
        //            for (int i = 0; i < item.Count; i++)
        //            {
        //                var X = (item[i].X - minX) * prop;
        //                var Y = (maxY - minY - (item[i].Y - minY)) * prop;
        //                var hexPosition1 = HexagonFunction.PixelToHexagon(layout,
        //                                         new Model.Point(X, Y));
        //                ret.Add(hexPosition1);
        //                PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(layout, hexPosition1).X, HexagonFunction.HexagonToPixel(layout, hexPosition1).Y));
        //                if (i != 0 && hexPosition1 != HexAnterior)
        //                {
        //                    linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);
        //                    poligon.AddRange(linea);
        //                    ret.AddRange(linea);

        //                }
        //                HexAnterior = HexagonFunction.PixelToHexagon(layout,
        //                                         new Model.Point(X, Y));
        //            }
        //            if (layout.FillPolygon)
        //                PaintHexInsidePolygon(PointsHexCornes, ref ret, layout);

        //        }
        //    }

        //    return ret.Distinct().ToList();
        //}

        public static List<Hex> HexMapGeoJSon(string PathJsonMap, ref Layout Layout)
        {
            List<Hex> ret = new List<Hex>();

            var PolygonToTake = new List<Polygon>();
            var PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
            var Positions = new List<IPosition>();
            var PoliygonList = new List<List<Model.Point>>();
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

                                Positions.Add(Point);
                            }
                            PoliygonList.Add(Figure);
                        }


                    }

                }
            }

            ret = HexListFomPolygon(PoliygonList, ref Layout);

            return ret.Distinct().ToList();
        }
        public static List<Hex> HexListFomPolygon(List<List<Model.Point>> PointsToTransformToHex, ref Layout Layout)
        {
            List<Hex> ret = new List<Hex>();
            var PoligonList = PointsToTransformToHex.SelectMany(list => list)
  .Distinct()
  .ToList();
            var ImageDifinition = new ImageDefinition(PoligonList, Layout);

            Layout = new Layout(Layout.Flat, new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize), new PointF(ImageDifinition.TransformedWidth / 2f, ImageDifinition.TransformedHeigth / 2f), Layout.HexPerLine, ImageDifinition.TransformedWidth, ImageDifinition.TransformedHeigth, Layout.FillPolygon);

            ret = TransformPointsToHex(PointsToTransformToHex, ImageDifinition, Layout);
            //foreach (var item in PointsToTransformToHex)
            //{
            //    Hex HexAnterior = new Hex();

            //    var linea = new List<Hex>();
            //    var PointsHexCornes = new List<PointF>();
            //    for (int i = 0; i < item.Count; i++)
            //    {
            //        var X = (item[i].X - ImageDifinition.OriginalMinX) * ImageDifinition.ProportationToScale;
            //        var Y = (ImageDifinition.OriginalMaxY - item[i].Y ) * ImageDifinition.ProportationToScale;
            //        var hexPosition1 = HexagonFunction.PixelToHexagon(Layout,
            //                                 new Model.Point(X, Y));
            //        ret.Add(hexPosition1);
            //        PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(Layout, hexPosition1).X, HexagonFunction.HexagonToPixel(Layout, hexPosition1).Y));
            //        if (i != 0 && hexPosition1 != HexAnterior)
            //        {
            //            linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);

            //            ret.AddRange(linea);

            //        }
            //        HexAnterior = HexagonFunction.PixelToHexagon(Layout,
            //                                 new Model.Point(X, Y));
            //    }
            //    if (Layout.FillPolygon)
            //        PaintHexInsidePolygon(PointsHexCornes, ref ret, Layout);

            //}


            return ret.Distinct().ToList();

        }
        public static List<Hex> TransformPointsToHex(List<List<Model.Point>> PointsToTransformToHex,
            ImageDefinition ImageDefinition, Layout Layout)
        {
            var ret = new List<Hex>();
            foreach (var item in PointsToTransformToHex)
            {
                Hex HexAnterior = new Hex();

                var linea = new List<Hex>();
                var PointsHexCornes = new List<PointF>();
                for (int i = 0; i < item.Count; i++)
                {
                    var X = (item[i].X - ImageDefinition.OriginalMinX) * ImageDefinition.ProportationToScale ;
                    var Y = (ImageDefinition.OriginalMaxY - item[i].Y) * ImageDefinition.ProportationToScale;
                    var hexPosition1 = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                    ret.Add(hexPosition1);
                    PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(Layout, hexPosition1).X, HexagonFunction.HexagonToPixel(Layout, hexPosition1).Y));
                    if (i != 0 && hexPosition1 != HexAnterior)
                    {
                        linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);

                        ret.AddRange(linea);

                    }
                    HexAnterior = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                }
                if (Layout.FillPolygon)
                    PaintHexInsidePolygon(PointsHexCornes, ref ret, Layout);

            }
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
