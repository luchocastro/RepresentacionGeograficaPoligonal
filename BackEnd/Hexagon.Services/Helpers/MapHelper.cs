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
using Hexagon.Shared.DTOs;

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

        
        public static List<Hex> HexMapGeoJSon(string PathJsonMap, ref HexagonGrid HexagonGrid)
        {
            List<Hex> ret = new List<Hex>();
            Layout Layout = HexagonGrid.Layout;

            var NameOfGroup = "";// Layout.MapDefinition.ColumnForMapGroup;
            var PolygonToTake = new List<Polygon>();
            var PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
            var Positions = new List<IPosition>();
            var PoliygonList = new List<List<Model.Point>>();
            var EventPoints = new List<EventPoint>();
            HexagonGrid.PuntosACalcular = new List<EventPoint>();
            var ColmnsForFunctionParaQueCompile = new string[] { "", "", "" };
            using (StreamReader file = File.OpenText(PathJsonMap))
            {
                var mapa = file.ReadToEnd();
                var feature = JsonConvert.DeserializeObject<FeatureCollection>(mapa);

                foreach (var featureItem in feature.Features)
                {

                    if (featureItem.Geometry != null)
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
                                SingleEvent SingleEvent = new SingleEvent();
                                PoliygonList.Add(Figure);
                                EventPoint EventPoint = new EventPoint();
                                EventPoint.Description = featureItem.Properties[Layout.MapDefinition.ColumnForMapGroup].ToString();
                                var Values = new List<string>();
                                foreach (var ValueNames in new string[] {"ACA","VAN","LAS", "" })
                                {

                                    Values.Add(featureItem.Properties.FirstOrDefault(value => value.Key == ValueNames).Value.ToString());

                                }
                                SingleEvent.values = Values;
                                if (EventPoint.Values == null)
                                    EventPoint.Values = new List<SingleEvent>();
                                EventPoint.GroupPoints = new List<Model.Point>();
                                EventPoint.GroupPoints.AddRange(Figure);
                                HexagonGrid.PuntosACalcular.Add(EventPoint);

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
                                EventPoint EventPoint = new EventPoint();
                                EventPoint.Description = featureItem.Properties[Layout.MapDefinition.ColumnForMapGroup].ToString();
                                var Values = new List<string>();
                                SingleEvent SingleEvent = new SingleEvent ();
                                //foreach (var ValueNames in Layout.MapDefinition.ColumnsNameForFuntion)
                                foreach (var ValueNames in ColmnsForFunctionParaQueCompile)
                                {

                                    Values.Add(featureItem.Properties.FirstOrDefault(value => value.Key == ValueNames).Value.ToString());

                                }
                                if (EventPoint.Values == null)
                                    EventPoint.Values = new List<SingleEvent>();
                                EventPoint.Values.Add(SingleEvent) ;
                                EventPoint.GroupPoints = new List<Model.Point>();
                                EventPoint.GroupPoints.AddRange(Figure);
                                HexagonGrid.PuntosACalcular.Add(EventPoint);


                            }


                        }
                        if (typeof(LineString) == featureItem.Geometry.GetType())
                        {
                            LineString Polygon = featureItem.Geometry as LineString ;
                            var Figure = new List<Model.Point>();

                            foreach (var CordPolygon in Polygon.Coordinates
                                )
                            {
                                 
                                    Figure.Add(new Model.Point((float)CordPolygon.Longitude, (float)CordPolygon.Latitude));

                                    Positions.Add(CordPolygon);
                                
                            }
                            
                            PoliygonList.Add(Figure);
                            EventPoint EventPoint = new EventPoint();
                            EventPoint.Description = featureItem.Properties[Layout.MapDefinition.ColumnForMapGroup].ToString();
                            var Values = new List<string>();
                            SingleEvent SingleEvent = new SingleEvent();
                            //foreach (var ValueNames in Layout.MapDefinition.ColumnsNameForFuntion)
                            foreach (var ValueNames in ColmnsForFunctionParaQueCompile)
                            {
                                

                                Values.Add(featureItem.Properties.FirstOrDefault(value => value.Key == ValueNames).Value.ToString());

                            }
                            SingleEvent.values = Values;
                            if (EventPoint.Values == null)
                                EventPoint.Values = new List<SingleEvent>();
                            EventPoint.Values.Add(SingleEvent);
                            EventPoint.GroupPoints = new List<Model.Point>();
                            EventPoint.GroupPoints.AddRange(Figure);

                            
                            HexagonGrid.PuntosACalcular.Add(EventPoint);


                        }
                        //event point
                    }
                }
            }
            HexagonGrid.Layout = Layout;
            //ret = HexListFomPolygon(PoliygonList, ref HexagonGrid );
            HexagonGrid.HexagonMap = ret;
            
            return ret.Distinct().ToList();
        }
        public static List<Hex> HexListFomPolygon(NativeFile NativeFile, ref HexagonGrid HexagonGrid)
        {
            Layout Layout = HexagonGrid.Layout;
            List<Hex> ret = new List<Hex>();
            
            var PoligonList = HexagonGrid.PuntosACalcular.SelectMany(x => x.GroupPoints).Distinct ().ToList();
            
            var ImageDifinition = new ImageDefinition(PoligonList, Layout);

            Layout = new Layout(Layout.Flat, new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize), new PointF(ImageDifinition.TransformedWidth / 2f, ImageDifinition.TransformedHeigth / 2f), Layout.HexPerLine, ImageDifinition.TransformedWidth, ImageDifinition.TransformedHeigth, Layout.FillPolygon);
            HexagonGrid.Layout=  Layout;
            ret = TransformPointsToHex(ref HexagonGrid, ImageDifinition );
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
            HexagonGrid.Layout = Layout;

            return ret.Distinct().ToList();

        }
        public static List<Hex> TransformPointsToHex(ref HexagonGrid HexagonGrid,
            ImageDefinition ImageDefinition)
        {
            Layout Layout = HexagonGrid.Layout;
            var ret = new List<Hex>();
            var Origin = new System.Drawing.PointF((ImageDefinition.TransformedWidth + ImageDefinition.HexagonSize * 4f) / 2,
                    (ImageDefinition.TransformedHeigth + MathF.Sqrt(3) * ImageDefinition.HexagonSize * 2f) / 2);
            Layout.Origin = Origin;
 
            foreach (var item in HexagonGrid.PuntosACalcular)
            {
                Hex HexAnterior = new Hex();

                var linea = new List<Hex>();
                var PointsHexCornes = new List<PointF>();
                for (int i = 0; i < item.GroupPoints.Count; i++)
                {
                    var X = Layout.Size.X * 2 + (item.GroupPoints[i].X - ImageDefinition.OriginalMinX) * ImageDefinition.ProportationToScale;
                    var Y = MathF.Sqrt(3) * Layout.Size.Y + (ImageDefinition.OriginalMaxY - item.GroupPoints[i].Y) * ImageDefinition.ProportationToScale;
                    var hexPosition1 = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                    var Existe = false;
                    //if (ret.Contains(hexPosition1))
                    //{
                    //    Existe = true;
                    //    hexPosition1 = ret[ret.IndexOf(hexPosition1)];

                    //    hexPosition1.Values.Add(item);
                    //    ret[ret.IndexOf(hexPosition1)] = hexPosition1 ;

                    //}
                    //else
                    {
                        hexPosition1.Values = new List<EventPoint>();

                        hexPosition1.Values.Add(item);
                        ret.Add(hexPosition1);
                    }
                    PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(Layout, hexPosition1).X, HexagonFunction.HexagonToPixel(Layout, hexPosition1).Y));
                    if (i != 0 && hexPosition1 != HexAnterior)
                    {
                        linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);
                        linea.Remove(hexPosition1);
                        ret.AddRange(linea );

                    }
                    HexAnterior = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                }
                if (Layout.FillPolygon)
                    PaintHexInsidePolygon(PointsHexCornes, ref ret, Layout);

            }
            HexagonGrid.Layout = Layout;
            var Grupo = ret.GroupBy(x => x.ToString() );
            var RetGroup = new List<Hex>();
            foreach (var grupo in Grupo)
            {
                var hex = grupo.First();
                if (hex.Values == null)
                    hex.Values = new List<EventPoint>();
                var hexs = grupo.Select (X=>X).ToList() ;
                for (int i = 1; i < hexs.Count();i++)
                {
                    if(hexs[i].Values!= null)
                        hex.Values.AddRange(hexs[i].Values);

                }
                RetGroup.Add(hex);
            }


            return RetGroup;
                //return ret.GroupBy(x => x.R.ToString() + "-" + x.S.ToString().ToString() + "-" + x.Q.ToString().ToString())
            //    .Select(group => new Hex(group.First().Q, group.First().R, group.First().S)
            //    {
            //        Q = group.First().Q,
            //        S = group.First().S,
            //        R = group.First().R,
            //        Values = group.ToArray().SelectMany(x => x.Values).ToList()
            //    }).ToList();
        }
        public static List<Hex> TransformPointToHex(ref HexagonGrid HexagonGrid,
    ImageDefinition ImageDefinition)
        {
            Layout Layout = HexagonGrid.Layout;
            var ret = new List<Hex>();
            var Origin = new System.Drawing.PointF((ImageDefinition.TransformedWidth + ImageDefinition.HexagonSize * 4f) / 2,
                    (ImageDefinition.TransformedHeigth + MathF.Sqrt(3) * ImageDefinition.HexagonSize * 2f) / 2);
            Layout.Origin = Origin;

            foreach (var item in HexagonGrid.PuntosACalcular)
            {
                Hex HexAnterior = new Hex();

                var linea = new List<Hex>();
                var PointsHexCornes = new List<PointF>();
                for (int i = 0; i < item.GroupPoints.Count; i++)
                {
                    var X = Layout.Size.X * 2 + (item.GroupPoints[i].X - ImageDefinition.OriginalMinX) * ImageDefinition.ProportationToScale;
                    var Y = MathF.Sqrt(3) * Layout.Size.Y + (ImageDefinition.OriginalMaxY - item.GroupPoints[i].Y) * ImageDefinition.ProportationToScale;
                    var hexPosition1 = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                    if (hexPosition1.Values == null)
                        hexPosition1.Values = new List<EventPoint>();
                    hexPosition1.Values.Add(item);
                    ret.Add(hexPosition1);
                    PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(Layout, hexPosition1).X, HexagonFunction.HexagonToPixel(Layout, hexPosition1).Y));
                    if (i != 0 && hexPosition1 != HexAnterior)
                    {
                        linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);
                        linea.Remove(hexPosition1);
                        ret.AddRange(linea);

                    }
                    HexAnterior = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                }
                if (Layout.FillPolygon)
                    PaintHexInsidePolygon(PointsHexCornes, ref ret, Layout);

            }
            HexagonGrid.Layout = Layout;
            var Grupo = ret.GroupBy(x => x.Q.ToString() + "-" + x.R.ToString() + "-" + x.S.ToString());
            var RetGroup = new List<Hex>();
            foreach (var grupo in Grupo)
            {
                var hex = grupo.First();
                if (hex.Values == null)
                    hex.Values = new List<EventPoint>();
                var hexs = grupo.Select(X => X).ToList();
                for (int i = 1; i < hexs.Count(); i++)
                {
                    if (hexs[i].Values != null)
                        hex.Values.AddRange(hexs[i].Values);

                }
                RetGroup.Add(hex);
            }


            return RetGroup;
            //return ret.GroupBy(x => x.R.ToString() + "-" + x.S.ToString().ToString() + "-" + x.Q.ToString().ToString())
            //    .Select(group => new Hex(group.First().Q, group.First().R, group.First().S)
            //    {
            //        Q = group.First().Q,
            //        S = group.First().S,
            //        R = group.First().R,
            //        Values = group.ToArray().SelectMany(x => x.Values).ToList()
            //    }).ToList();
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
        public static void HexMap(NativeFileDTO NativeFileDTO, ref HexagonGrid HexagonGrid, ref HexagonDetails HexagonDetails)
        {
            List<Hex> ret = new List<Hex>();
            Layout Layout = HexagonGrid.Layout;

    var ColmnsForFunctionParaQueCompile = new string[] { "", "", "" };
    var PoliygonList = new List<List<Model.Point>>();

            if (NativeFileDTO.Content.Count() == 0)
                return;

            var NameOfGroup = Layout.MapDefinition.ColumnForMapGroup;
            var ColumnX = Layout.MapDefinition.ColumnNameForX;
            var ColumnY = Layout.MapDefinition.ColumnNameForY;
            var ColumnValue = ColmnsForFunctionParaQueCompile [0];
            var EventPoints = new List<EventPoint>();
             var QColumns = NativeFileDTO.Columns.Count();
            if (NameOfGroup != "")
            {
                var col = NativeFileDTO.Columns.Where(x => x.Name == NameOfGroup).FirstOrDefault();

                var pols = NativeFileDTO.Content.Select(x => x.Fieds[col.OriginalPosition]);
                PoliygonList = pols.Select(X => X.Split(",").Select(y => new Model.Point((float)Convert.ToDouble(y[0]), (float)Convert.ToDouble(y[1]))).ToList()).ToList();
                Layout.PaintLines = true;
                //ret = MapHelper.HexListFomPolygon(PoliygonList, ref HexagonGrid);

            }
            else
            {
                var colX = NativeFileDTO.Columns.Where(x => x.Name == ColumnX).FirstOrDefault();
                var colY = NativeFileDTO.Columns.Where(x => x.Name == ColumnY).FirstOrDefault();
                var colvalue = NativeFileDTO.Columns.Where(x => x.Name == ColumnValue).FirstOrDefault();
                Layout.PaintLines = false;
                //EventPoints = NativeFileDTO.Content.Where(x => x.Fieds.Count() == NativeFileDTO.Columns.Count).Select(y => new Model.EventPoint() { PositionInMeters = new PointF((float)Convert.ToDouble(y.Fieds[colX.OriginalPosition]), (float)Convert.ToDouble(y.Fieds[colY.OriginalPosition])), Value = ((float)Convert.ToDouble(y.Fieds[colvalue.OriginalPosition])) }).ToList();
                var PoliygonListk = NativeFileDTO.Content.Where(x => x.Fieds.Count() == NativeFileDTO.Columns.Count).Select(y => new Model.Point((float)Convert.ToDouble(y.Fieds[colX.OriginalPosition]), (float)Convert.ToDouble(y.Fieds[colY.OriginalPosition]))).ToList();
                //for (int i = 0; i < EventPoints.Count (); i++)
                //{


                //    EventPoints[i].Values.Add(new SingleEvent()
                //    {
                //        values = new string[] { EventPoints[i].Value.ToString() }.ToList(),
                //        Point = new Model.Point(EventPoints[i].PositionInMeters.X, EventPoints[i].PositionInMeters.Y)
                //    });

                //}

                var ImageDifinition = new ImageDefinition(PoliygonListk, Layout);

                Layout = new Layout(Layout.Flat, new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize), new PointF(ImageDifinition.TransformedWidth / 2f, ImageDifinition.TransformedHeigth / 2f), Layout.HexPerLine, ImageDifinition.TransformedWidth, ImageDifinition.TransformedHeigth, Layout.FillPolygon);
                HexagonGrid.Layout = Layout;
                HexagonGrid.PuntosACalcular = EventPoints;
                var QLines = NativeFileDTO.Content.Count();
                foreach (var Line in NativeFileDTO.Content)
                {
                    if(QColumns!=Line.Fieds.Count())
                        continue;
                    var EventPoint = new EventPoint() { PositionInMeters = new PointF((float)Convert.ToDouble(Line.Fieds[colX.OriginalPosition]), (float)Convert.ToDouble(Line.Fieds[colY.OriginalPosition])), Value = ((float)Convert.ToDouble(Line.Fieds[colvalue.OriginalPosition])) };
                    var X = Layout.Size.X * 2 + (EventPoint.PositionInMeters.X - ImageDifinition.OriginalMinX) * ImageDifinition.ProportationToScale;
                    var Y = MathF.Sqrt(3) * Layout.Size.Y + (ImageDifinition.OriginalMaxY - EventPoint.PositionInMeters.Y) * ImageDifinition.ProportationToScale;
                    var hexPosition1 = HexagonFunction.PixelToHexagon(Layout,
                                             new Model.Point(X, Y));
                    var Eventpoints = new List<EventPoint>();
                    HexagonDetail HexagonDetail = null;
                    var ListLine = new List<Line>();
                    if (!ret.Contains(hexPosition1))
                    {//if (hexPosition1.Values == null)
                        Eventpoints = new List<EventPoint>();

                        // Eventpoints.Add(EventPoints[i]);
                        hexPosition1.Value = EventPoint.Value;
                        //hexPosition1.Values = EventPoints;
                        //ListLine.Add(new Line( Line.Number, Line.Fieds));
                        //HexagonDetail = new HexagonDetail(hexPosition1, ListLine);

                        //HexagonDetails.Add(HexagonDetail)  ;
                        //ret.Add(hexPosition1);
                    }
                    else
                    {

                        hexPosition1 = ret[ret.IndexOf(hexPosition1)];
                        // = hexPosition1.Values;
                        //Eventpoints.Add(EventPoints[i]);
                        //aca hay que calcular y guardar los valores

                        ListLine.Add(new Line(Line.Number, Line.Fieds));
                        ////var indexOF = HexagonDetails.List.FindLastIndex( x => x.Q == hexPosition1.Q && x.R == hexPosition1.R
                        ////      && x.S == hexPosition1.S);

                        ////HexagonDetails.List[indexOF].Lines.Add(new Line() { Fieds = Line.Fieds });

                        hexPosition1.Value = EventPoint.Value;
                        //hexPosition1.Values = EventPoints;
                        ret[ret.IndexOf(hexPosition1)] = hexPosition1;
                    }

                }
            }
            HexagonGrid.HexagonMap = ret;

            // return ret.Distinct().ToList();
        }

    }

}
