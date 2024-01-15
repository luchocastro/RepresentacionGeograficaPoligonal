using System;
using System.Collections.Generic;
using System.IO;
using Hexagon.Model;
using Hexagon.Shared.DTOs;
using Newtonsoft.Json;
using System.Reflection;
using Hexagon.Model.Models;

using Newtonsoft ;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hexagon.Model.Repository;
using System.Globalization;

namespace Hexagon.Services.Helpers
{
    public class FilesHelper
    {
        public static Line LineToField(string[] Lista, long Number)
        {


            Line line = new Line(Number, Lista );

            return line;

        }
        public static List<DataFileConfigurationDTO> ReadDataFileConfiguration(string Path)
        {
            List<DataFileConfigurationDTO> DataFileConfiguration = new List<DataFileConfigurationDTO>();

            string Configuration = File.ReadAllText(Path);
            DataFileConfiguration = JsonConvert.DeserializeObject<List<DataFileConfigurationDTO>>(Configuration);
            return DataFileConfiguration;
        }
        public static List<Column> ReadFileDef(string PathFile)
        {
            List<Column> ret = new List<Column>();
            using (StreamReader file = File.OpenText(PathFile))
            {
                var def = JsonConvert.DeserializeObject<List<Column>>(file.ReadToEnd());
                ret = def;
            }
            return ret;
        }
        public static List<EventPoint> ReadDataFile(string ColumnNameX, string ColumnNameY, DateTime eventTime, List<string> ColumnNamesData, string PathJsonFile)
        {
            List<EventPoint> ret = new List<EventPoint>();


            using (StreamReader file = File.OpenText(PathJsonFile))
            {

                var NativeFile = JsonConvert.DeserializeObject<List<Line>>(file.ReadToEnd());

                for (int i = 0; i < NativeFile.Count(); i++)
                {
                    var a = NativeFile[i].Fieds;
                    EventPoint ev = new EventPoint();
                    ev.EventTime = eventTime;
                    ev.Description = "";
                    ret.Add(ev);


                }



                return ret;
            }
        }
        public static void GenerateXY(Column ColumnX, Column ColumnY, Column ColumnXY, IDataRepository<ColumnDTO, Column> dataRepository  , string decimalSepar=".", bool DeleteIfExist=true)
        {
            var t = Task.Run(() =>
            {
                bool merge = true;
                bool Exist = false;
                if (File.Exists(ColumnXY.PathFields))
                    if (DeleteIfExist)
                        File.Delete(ColumnXY.PathFields);
                    else
                        return;
                var ColumnPointDistance = new Column {ParentID= ColumnXY.ParentID, Name = "MAE", NumberOfRows = ColumnY.NumberOfRows, ActionToDoWithUncasted = EnumActionToDoWithUncasted.DeleteData }  ;
                ColumnPointDistance.ID = dataRepository.GenerateFullID(ColumnPointDistance);
                ColumnPointDistance.Path = Path.Combine( dataRepository.ClassLocation(ColumnPointDistance), ColumnPointDistance.Name + ".Hex.Json");
                ColumnPointDistance.PathFields = Path.Combine(dataRepository.ClassLocation(ColumnPointDistance), typeof(Field).Name, ColumnPointDistance.Name + ".Hex.Json");  


                using StreamReader StreamReaderX = new StreamReader(ColumnX.PathFields);
                using StreamReader StreamReaderY = new StreamReader(ColumnY.PathFields);
                using StreamWriter StreamWriter = new StreamWriter(ColumnXY.PathFields, true);
                using StreamWriter StreamWriterPoints = new StreamWriter(ColumnPointDistance.PathFields, true);
                try
            {



                ColumnX.MinValue = double.MaxValue;
                ColumnX.MaxValue = double.MinValue;

                ColumnY.MinValue = double.MaxValue;
                ColumnY.MaxValue = double.MinValue;
                
                while (merge)
                {
                    var x = "";
                    var y = "";

                    var fieldX = new Field();
                    var fieldY = new Field();
                    x = StreamReaderX.ReadLine();
                    if (x == null)
                        merge = false;
                    else
                        fieldX = new Field().FromString(x);
                    y = StreamReaderY.ReadLine();
                    if (y == null)
                        merge = false;
                    else
                        fieldY = new Field().FromString(y);

                    if (merge && fieldY.Index == fieldX.Index)
                    {
                        fieldX.Value = fieldX.Value.ToString().Replace(decimalSepar == "." ? "," : ".", "").Replace(",", ".");
                        var xNum = Convert.ToDouble(fieldX.Value, CultureInfo.InvariantCulture);
                        fieldY.Value = fieldY.Value.ToString().Replace(decimalSepar == "." ? "," : ".", "").Replace(",", ".");
                        var yNum = Convert.ToDouble(fieldY.Value, CultureInfo.InvariantCulture);

                        ColumnX.MinValue = (double)ColumnX.MinValue > xNum ? xNum : ColumnX.MinValue;
                        ColumnX.MaxValue = (double)ColumnX.MaxValue < xNum ? xNum : ColumnX.MaxValue;
                        ColumnY.MinValue = (double)ColumnY.MinValue > yNum ? yNum : ColumnY.MinValue;
                        ColumnY.MaxValue = (double)ColumnY.MaxValue < yNum ? yNum : ColumnX.MaxValue;
                        var Point = new Model.Point((float)xNum,
                            (float)yNum);
                        var fieldXY = new Field() { Index = fieldY.Index, Value = "(" + xNum.ToString(CultureInfo.InvariantCulture) + ":" + yNum.ToString(CultureInfo.InvariantCulture) + ")" };
                        StreamWriter.WriteLine(fieldXY.ToString());
                    }
                }
                    StreamWriter.Dispose();
                    using StreamReader StreamReaderXY = new StreamReader(ColumnXY.PathFields);
 
                    var ColumnXMinValue = (Double)ColumnX.MinValue  ;
                    var ColumnXMaxValue = (Double)ColumnX.MaxValue ;
                    var ColumnYMinValue = (Double)ColumnY.MinValue  ;
                        var ColumnYMaxValue = (Double)ColumnY.MaxValue  ;
                    ColumnPointDistance.MinValue = Double.MaxValue;
                    ColumnPointDistance.MaxValue= Double.MinValue;
                    if (File.Exists(ColumnPointDistance.PathFields))
                         File.Delete(ColumnPointDistance.PathFields);
                     merge = true;
                    var SUMMAE = 0f;
                    try
                    {
                        while (merge)
                        {
                            var ActualRead = "";
                            ActualRead = StreamReaderXY.ReadLine();

                            if (ActualRead == null)
                                merge = false;
                            else
                            {
                                var fieldXY = new Field().FromString(ActualRead);
                                var Point = new Point( );

                                var Xs = new double[] { Point.X, ColumnXMaxValue, ColumnXMinValue };
                                var Ys = new double[] { Point.Y, ColumnYMinValue, ColumnYMaxValue };
                                var Points = new Point[] { new Point(ColumnXMaxValue, ColumnYMaxValue),
                                new Point(ColumnXMaxValue, ColumnYMinValue), new Point(ColumnXMinValue, ColumnYMaxValue),
                                new Point(ColumnXMinValue, ColumnYMinValue)  };
                                var MAE = (double)Points.Select(x => Math.Sqrt(Math.Pow(Point.X - x.X, 2) + Math.Pow(Point.Y - x.Y, 2))).Min();
                                var fieldMAE = new Field { Index = fieldXY.Index, Value = MAE };
                                ColumnPointDistance.MinValue = (double)ColumnPointDistance.MinValue > MAE ? MAE : ColumnPointDistance.MinValue;
                                ColumnPointDistance.MaxValue = (double)ColumnPointDistance.MaxValue < MAE ? MAE : ColumnPointDistance.MaxValue;

                                StreamWriterPoints.WriteLine(fieldMAE.ToString());

                            }

                        }
                    }
                    finally { StreamReaderXY.Dispose(); }
                    //var clusters = Hexagon.Cluster.DBSCAN.Get(points, eps, minPts);
                }
                finally
                {
                    StreamReaderX.Dispose();
                    StreamReaderY.Dispose();
                    StreamWriter.Dispose();
                    StreamWriterPoints.Dispose();
                }
                dataRepository.Add(ColumnY);
                dataRepository.Add(ColumnX);
                dataRepository.Add(ColumnXY);

                dataRepository.Add(ColumnPointDistance);
                 
            });


            //GetClusters(points, eps, minPts)
        }

     
        public void Algo(string HexFileID, long MinRowsToDiscard, long MaxRowsToTake, IDataRepository<ColumnDTO, Column> ColumnManager , IMapper _Mapper   )
        {
            var z = Task.Run(() =>
            {
                var columns = ColumnManager.GetColectionFromParent(HexFileID);
                if (columns != null && columns.Count() > 0)
                {

                    long Qfields = columns.ToList()[0].NumberOfRows;
                    long MinRows = MinRowsToDiscard ;
                    long MaxRows = MaxRowsToTake;
                    long Total = (long)Math.Floor(Math.Sqrt(Qfields / 2.0));
                    var QRows = new List<long>();
                    Random rnd = new Random(DateTime.Now.Millisecond);
                    if (Qfields < MaxRows)
                    {

                        for (var i = 0; i < Total; i++)
                        {
                            QRows.Add(rnd.Next(0, (int)Qfields - 1));
                        }
                        QRows = QRows.Distinct().OrderBy(x => x).ToList();

                        foreach (var col in columns)
                        {

                            using (var FileStream = new StreamReader(col.PathFields))
                            {
                                col.PathSampleFields = Path.Combine(Path.GetDirectoryName(col.PathFields), "Sample" + Path.GetFileName(col.PathFields));
                                using (var File = new StreamWriter(col.PathSampleFields))
                                {
                                    int actualpos = 0;
                                    while (true)
                                    {

                                        var ToField= FileStream.ReadLine();
                                        if (ToField == null) break;
                                        var Field = new Field().FromString(ToField);
                                        if (QRows[actualpos]  == Field.Index)
                                        {
                                            actualpos++;
                                            File.Write(ToField);
                                            File.WriteLine();
                                        }
                                    }
                                    File.Close();
                                    File.Dispose();
                                }
                                ColumnManager.Add(_Mapper.Map<Column>(col));
                                FileStream.Close();
                                FileStream.Dispose();
                            }
                        }


                    }
                }
            });
        
    }

        public static void SaveFile<T>(string Destination, T Type)  
        {
            string ToSave = JsonConvert.SerializeObject (Type);
            using (StreamWriter StreamWriter = new StreamWriter(Destination))
            {
                StreamWriter.Write(ToSave);
                StreamWriter.Close();
            }
        }
    }
}