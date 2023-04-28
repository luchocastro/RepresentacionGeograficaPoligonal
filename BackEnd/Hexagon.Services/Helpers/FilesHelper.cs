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