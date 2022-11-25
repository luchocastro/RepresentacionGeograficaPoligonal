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

namespace Hexagon.Services.Helpers
{
    public class FilesHelper
    {
        public static List<DataFileConfigurationDTO> ReadDataFileConfiguration()
        {
            List<DataFileConfigurationDTO> DataFileConfiguration = new List<DataFileConfigurationDTO>();

            string ApiDirectory = Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var FileConfiguration = Path.Combine(ApiDirectory, ServicesConstants.FilesDirectory, ServicesConstants.ConfigurationFilesName);
            string Configuration = File.ReadAllText(FileConfiguration);
            DataFileConfiguration = JsonConvert.DeserializeObject<List<DataFileConfigurationDTO>>(Configuration);
            return DataFileConfiguration;
        }

        public static List<EventPoint> ReadDataFile(string ColumnNameX, string ColumnNameY, DateTime eventTime, List<string> ColumnNamesData, string PathJsonFile)
        {
            List<EventPoint> ret = new List<EventPoint>();

            string ApiDirectory = Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //JObject my_obj = JsonConvert.DeserializeObject<JObject>(PathJsonFile);

            //using (FileStream file = System.IO.File.OpenRead(PathJsonFile))
            //{
            //    foreach (KeyValuePair<string, JToken> sub_obj in (JObject)my_obj["ADDRESS_MAP"])
            //{
            //    Console.WriteLine(sub_obj.Key);
            //}

            using (StreamReader file = File.OpenText(PathJsonFile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {

                    var o2 = (JObject)JToken.ReadFrom(reader);
                    for (int i = 0; i < o2.Count; i++)
                    {
                        EventPoint ev = new EventPoint();
                        ev.EventTime = eventTime;
                        ev.Description = "";
                        var datos = ((JObject)(o2).First);
                        ev.Value = ((JObject)datos).Value<float>(ColumnNamesData[0]);
                        ret.Add(ev);
                    }

                }



                return ret;
            }
        }
    }
}