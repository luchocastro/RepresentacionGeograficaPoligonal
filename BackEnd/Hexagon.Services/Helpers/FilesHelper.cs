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
        public static Line LineToField(string[] Lista, UInt64 Number)
        {


            Line line = new Line(Number, Lista.Select(x => x).ToArray());

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
        public static List<ProyectDataDTO> ReadProject(string PathProjects, string ProjectName = ServicesConstants.NicFolder + "_1", AnalizedFileDTO AnalizedFileDTO = null)
        {
            string name = Path.Combine(PathProjects);
            ;
            List<ProyectDataDTO> ret = new List<ProyectDataDTO>();
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
            var def = new ProyectDataDTO(); 
            
                var Datafolder = "Data";
                if (AnalizedFileDTO != null && AnalizedFileDTO.NicName != "")
                    Datafolder = AnalizedFileDTO.NicName;
                if (!Directory.Exists(Path.Combine(PathProjects, ProjectName)))
                    Directory.CreateDirectory(Path.Combine(PathProjects, ProjectName));
            if (!Directory.Exists(Path.Combine(PathProjects, ProjectName, Datafolder)))
                Directory.CreateDirectory(Path.Combine(PathProjects, ProjectName, Datafolder));
            if (!Directory.Exists(Path.Combine(PathProjects, ProjectName, Datafolder, ServicesConstants.MapsDirectory)))
                Directory.CreateDirectory(Path.Combine(PathProjects, ProjectName, Datafolder, ServicesConstants.MapsDirectory));
            if (!Directory.Exists(Path.Combine(PathProjects, ProjectName, Datafolder, ServicesConstants.FilesDirectory)))
                Directory.CreateDirectory(Path.Combine(PathProjects, ProjectName, Datafolder, ServicesConstants.FilesDirectory));
            if (!Directory.Exists(Path.Combine(PathProjects, ProjectName, Datafolder, ServicesConstants.NativesDirectory)))
                Directory.CreateDirectory(Path.Combine(PathProjects, ProjectName, Datafolder, ServicesConstants.NativesDirectory));


            


            if (File.Exists(Path.Combine(PathProjects, ProjectName, ServicesConstants.NicProjectData)))
            {
                using (StreamReader file = File.OpenText(Path.Combine(PathProjects, ProjectName, ServicesConstants.NicProjectData)))
                {
                    def = JsonConvert.DeserializeObject<ProyectDataDTO>(file.ReadToEnd());

                }
            }
            else
            {
                def.Location = new LocationDTO(Path.Combine(PathProjects, ProjectName), ServicesConstants.MapsDirectory, ServicesConstants.FilesDirectory, ServicesConstants.NativesDirectory);
                def.Name = ProjectName;
            }
            if (AnalizedFileDTO != null)
            {
                if (def.AnalizedFiles == null)
                    def.AnalizedFiles = new List<AnalizedFileDTO>();
                
                    if (def.AnalizedFiles.Where(x => x.OriginalFileName == AnalizedFileDTO.OriginalFileName).Count() == 0)
                    {

                        def.AnalizedFiles.Add(AnalizedFileDTO);
                    }
                
            }

            using (StreamWriter StreamWriter = new StreamWriter(Path.Combine(PathProjects, def.Name, ServicesConstants.NicProjectData)))
            {
                StreamWriter.Write(JsonConvert.SerializeObject(def));
                StreamWriter.Close();
            }

            ret.Add(def);

            foreach (var dir in Directory.GetDirectories(name))
            {

                string nameFileProject = Path.Combine(dir, ServicesConstants.NicProjectData);

                if (File.Exists(nameFileProject))
                {
                    using (StreamReader file = File.OpenText(nameFileProject))
                    {
                        var def1 = JsonConvert.DeserializeObject<ProyectDataDTO>(file.ReadToEnd());
                        ret.Add(def1);
                    }
                }

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