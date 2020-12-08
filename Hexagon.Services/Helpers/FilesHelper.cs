using System;
using System.Collections.Generic;
using System.IO;
using Hexagon.Model;
using Hexagon.Shared.DTOs;
using Newtonsoft.Json;
using System.Reflection;
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
    }
}
