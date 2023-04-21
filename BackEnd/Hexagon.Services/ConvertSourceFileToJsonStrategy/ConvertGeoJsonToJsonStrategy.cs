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
using System.Globalization;
using System.Threading.Tasks;
using Hexagon.AsyncIO;

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
        List<Polygon> PolygonToTake = new List<Polygon>();
        List<GeoJSON.Net.Geometry.Point> PoitsToTake = new List<GeoJSON.Net.Geometry.Point>();
        List<IPosition> Positions = new List<IPosition>();
        List<List<Model.Point>> PoliygonList = new List<List<Model.Point>>();
        List<EventPoint> EventPoints = new List<EventPoint>();
        bool primera = true;
        NativeFile NativeFile = new NativeFile();
        int Step = 1;
        List<Line> Lineas = new List<Line>();
        List<string> Columns = new List<string>();
        List<Column> ColumnsForModel = new List<Column>();
        int FirstNRows = 50;
        public async Task<NativeFile> DoFromFileAsync(string PathFileOrigen, DataFileConfiguration FileData, int FirstNRows = 0)

        {
        
            var AsyncIO = new AsyncIO<Feature>(PathFileOrigen);
            AsyncIO.ReadArrayElement += AsyncIO_ReadArrayFeature;
            await AsyncIO.ReadJsonArrayAsync("features");


            NativeFile.Content = Lineas;
            //for (int i = 0; i < ColumnsForModel.Count(); i++)
            //{
            //    var col = ColumnsForModel[i];
            //    var FieldsEv = Step > 100 ? 100 : Step;
            //    var datatypes = FindTypesAllowed.TypesPrincipals(ColumnsArray[i].DataTypeFinded, FieldsEv);
            //    ColumnsForModel[i].DataTypeFinded=datatypes;
            //}
            NativeFile.DataFileConfiguration = FileData;
            NativeFile.Columns = ColumnsForModel;
            NativeFile.IsPolygon = true;
            return NativeFile;


        }

        private void AsyncIO_ReadArrayFeature(object sender, AsyncIO<Feature>.EventReadArray e)
        {

            var featureItem = e.Item;
            {
                var Column = new Column();
                int pos = 0;
                if (primera)
                {
                    foreach (var ValueNames in featureItem.Properties.Keys)
                    {
                        Column = new Column();
                        Column.Name = ValueNames;
                        Column.OriginalPosition = pos;
                        pos++;
                        var Types = Column.DataTypeFinded;
                        ColumnsForModel.Add(Column);
                        Columns.Add(ValueNames);
                        primera = false;
                    }
                    Columns.Add("Geometry");
                    Column = new Column();
                    Column.Name = "Geometry";
                    Column.OriginalPosition = pos;
                    ColumnsForModel.Add(Column);
                    //ColumnsArray = ColumnsForModel.ToArray();
                    //Lineas.Add(Helpers.FilesHelper.LineToField(Columns.ToArray(), (ulong)Step));
                }
                var Values = new List<string>();

                pos = 0;
                foreach (var ValueNames in featureItem.Properties.Keys)
                {
                    object value = new object();
                    featureItem.Properties.TryGetValue(ValueNames, out value);
                    if (value == null)
                        value = "";
                    //if (Step<100)
                    //{ 
                    //var Parsed = FindTypesAllowed.GetTypesAllows(value.ToString(), FileData);

                    //ColumnsArray[pos].DataTypeFinded.AddRange(Parsed);
                    //}
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
                Values.Add(String.Join(",", Figure.Select(x => x.X.ToString(CultureInfo.InvariantCulture) + ":" + x.Y.ToString(CultureInfo.InvariantCulture)).ToArray()));

                Lineas.Add(new Line((long)Step, Values.ToArray()));
                Step++; 
                if (Step == FirstNRows)
                    e.Cancel = true ;
                

            }
        }

        public NativeFile DoFromFile(string PathFileOrigen, DataFileConfiguration FileData, int FirstNRows = 0)
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
            List<Column> ColumnsForModel = new List<Column>();
            Column[] ColumnsArray = null;
            int Step = 1; 
            using (StreamReader file = File.OpenText(PathFileOrigen))
            {
                var mapa = file.ReadToEnd();
                var feature = JsonConvert.DeserializeObject<FeatureCollection>(mapa);
                
                foreach (var featureItem in feature.Features)
                {
                    var Column = new Column();
                    int pos = 0;
                    if (primera)
                    {
                        foreach (var ValueNames in featureItem.Properties.Keys)
                        {
                            Column = new Column();
                            Column.Name = ValueNames;
                            Column.OriginalPosition = pos;
                            pos++;
                            var Types = Column.DataTypeFinded;
                            ColumnsForModel.Add(Column);
                            Columns.Add(ValueNames);
                            primera = false;
                        }
                        Columns.Add("Geometry");
                        Column = new Column();
                        Column.Name = "Geometry";
                        Column.OriginalPosition = pos;
                        ColumnsForModel.Add(Column);
                        //ColumnsArray = ColumnsForModel.ToArray();
                        //Lineas.Add(Helpers.FilesHelper.LineToField(Columns.ToArray(), (ulong)Step));
                    }
                    var Values = new List<string>();

                    pos = 0;
                    foreach (var ValueNames in featureItem.Properties.Keys)
                    {
                        object value = new object();
                        featureItem.Properties.TryGetValue(ValueNames, out value) ;
                        if (value == null)
                            value = "";
                        //if (Step<100)
                        //{ 
                        //var Parsed = FindTypesAllowed.GetTypesAllows(value.ToString(), FileData);
                        
                        //ColumnsArray[pos].DataTypeFinded.AddRange(Parsed);
                        //}
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
                    Values.Add(String.Join(",", Figure.Select(x => x.X.ToString(CultureInfo.InvariantCulture) + ":" + x.Y.ToString(CultureInfo.InvariantCulture)).ToArray()));
                   
                    Lineas.Add(new Line((long)Step, Values.ToArray() ));
                    if (Step == FirstNRows)
                        break;
                        Step++;
                    
                }
            }
            
            NativeFile.Content = Lineas;
            //for (int i = 0; i < ColumnsForModel.Count(); i++)
            //{
            //    var col = ColumnsForModel[i];
            //    var FieldsEv = Step > 100 ? 100 : Step;
            //    var datatypes = FindTypesAllowed.TypesPrincipals(ColumnsArray[i].DataTypeFinded, FieldsEv);
            //    ColumnsForModel[i].DataTypeFinded=datatypes;
            //}
            NativeFile.DataFileConfiguration = FileData;
            NativeFile.Columns = ColumnsForModel;
            NativeFile.IsPolygon = true;
            return NativeFile;


        }
    }
}
