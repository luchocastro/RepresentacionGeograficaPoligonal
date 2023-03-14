using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hexagon.Model.Models;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using Hexagon.Shared.CommonFunctions;
using Hexagon.Model;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;

namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    public class ConvertGeoJsonToJsonStrategy : IConvertSourceFileToJsonStrategy
    {
        public NativeJsonFile Do(string Base64File, DataFileConfiguration FileData )
        {            
            var Base64EncodedBytes = System.Convert.FromBase64String(Base64File);
            MemoryStream SourceFile = new MemoryStream(Base64EncodedBytes);
            object Delimiter = null;
            FileData.FileProperties.TryGetValue("Delimiter", out Delimiter) ;
            if (Delimiter == null)
                throw new Exception( ServicesConstants.DelimiterMissing);
            object HasTitleInDefinition = null;
            FileData.FileProperties.TryGetValue("HasTitle", out HasTitleInDefinition) ;
            bool HasTitle = HasTitleInDefinition != null && Convert.ToBoolean(HasTitleInDefinition.ToString());
            string []Columns  = null;
            int Step = 0;
            List<JObject> JObjectList = new List<JObject>();
            using (StreamReader StreamReader = new StreamReader(SourceFile))
            {
                String ActualLine = "";
                 while ((ActualLine = StreamReader.ReadLine()) != null)
                {
                    string[] DataInLine = ActualLine.Split(Delimiter.ToString());
                    int ColumnsQuantity = DataInLine.Length;
                    Step++;
                    if (Step == 1 )
                    {
                        if (HasTitle)
                        {
                            Columns = DataInLine;
                        }
                        else
                        {                            
                            Columns = new string[ColumnsQuantity];
                            for (int i = 0; i < ColumnsQuantity; i++)
                            {
                                Columns[i] = "Column-"+ i.ToString();
                            }
                        }
                       
                        
                    }
                    dynamic JObjectLine = new JObject();
                    if (Step > 1 || !HasTitle)
                    {
                        int i = 0;
                        foreach (string Data in DataInLine)
                        {

                            JObjectLine[Columns[i]] = new JValue(TypeFunction.GetTyped(Data)); ;
                            i++;
                        }
                        JObjectList.Add(JObjectLine);
                    }
                    
                }
            }
            var JSonFileConverted = Newtonsoft.Json.JsonConvert.SerializeObject(JObjectList);
            NativeJsonFile NativeJsonFile = new NativeJsonFile();

            var ColumnsForModel=new List<Column>();
            int pos = 0;
            foreach (var item in Columns)
            {
                ColumnsForModel.Add(new Column(item, pos, EnumActionToDoWithUncasted.DeleteData, EnumAlowedDataType.Character ));
                pos++;
            }
            NativeJsonFile.Content = JSonFileConverted;
           NativeJsonFile.Columns = ColumnsForModel;
            return NativeJsonFile;
        }
        public NativeFile DoFromFile(string PathFileOrigen, DataFileConfiguration FileData)
        {
            var PolygonToTake = new List<Polygon>();
            var PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
            var Positions = new List<IPosition>();
            var PoliygonList = new List<List<Model.Point>>();
            var EventPoints = new List<EventPoint>();
            bool primera = true;
            NativeFile NativeFile = new NativeFile();

            List<Line> Lineas = new List<Line>();
            List<string> Columns = new List<string>();
            using (StreamReader file = File.OpenText(PathFileOrigen))
            {
                var mapa = file.ReadToEnd();
                var feature = JsonConvert.DeserializeObject<FeatureCollection>(mapa);
                int Step = 0;
                 
                foreach (var featureItem in feature.Features)
                {
                    if (primera)
                    {
                        foreach (var ValueNames in featureItem.Properties.Keys)
                        {

                            Columns.Add(ValueNames);
                            primera = false;
                        }
                        Columns.Add("Geometry");
                        Lineas.Add(Helpers.FilesHelper.LineToField(Columns.ToArray(), (ulong)Step));
                    }
                    var Values = new List<string>();
                    foreach (var ValueNames in featureItem.Properties.Keys)
                    {
                        object value = new object();
                        featureItem.Properties.TryGetValue(ValueNames, out value) ;
                        if (value == null)
                            value = "";
                        Values.Add(value.ToString());

                    }

                    var Figure = new List<Model.Point>();
                    if (featureItem.Geometry != null)
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
                                        Figure.Add(new Model.Point((float)Point.Longitude, (float)Point.Latitude));
                                        Positions.Add(Point);
                                    }
                                }
                                //HexagonGrid.PuntosACalcular.Add(EventPoint);

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
                                    Figure.Add(new Model.Point((float)Point.Longitude, (float)Point.Latitude));

                                    Positions.Add(Point);
                                }
                                


                            }


                        }
                        if (typeof(LineString) == featureItem.Geometry.GetType())
                        {
                            LineString Polygon = featureItem.Geometry as LineString;
                            
                            foreach (var CordPolygon in Polygon.Coordinates
                                )
                            {

                                Figure.Add(new Model.Point((float)CordPolygon.Longitude, (float)CordPolygon.Latitude));

                            }
                        }
                        //event point
                    }
                    Values.Add(String.Join(",", Figure.Select(x => x.X.ToString() + ":" + x.Y.ToString()).ToArray()));
                   
                    Lineas.Add(Helpers.FilesHelper.LineToField(Values.ToArray(), (ulong)Step));
                    Step++;
                }
            }
            
            NativeFile.Content = Lineas;
            var ColumnsForModel = new List<Column>();
            int pos = 0;
            foreach (var item in Columns)
            {
                ColumnsForModel.Add(new Column(item, pos, EnumActionToDoWithUncasted.DeleteData, EnumAlowedDataType.Character));
                pos++;
            }

            NativeFile.Columns = ColumnsForModel;
            return NativeFile;


        }
    }
}
