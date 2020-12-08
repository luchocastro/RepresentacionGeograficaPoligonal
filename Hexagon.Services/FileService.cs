using Hexagon.Model.Models;
using Hexagon.Services.ConvertSourceFileToJsonStrategy;
using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using Hexagon.Services.Helpers;
using Hexagon.Services.Interfaces;

namespace Hexagon.Services
{
    public class FileService : IFileService
    {
        public NativeJsonFileDTO ConvertFile (string Base64File, Dictionary<string, object> FileData)
        {
            NativeJsonFile NativeJsonFile = new NativeJsonFile();

            NativeJsonFile = GetJsonSerializedFile(Base64File, FileData);
            return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };
        }
        public List<DataFileConfigurationDTO> GetDataFileConfiguration()
        {
            return FilesHelper.ReadDataFileConfiguration();
        }
        public NativeJsonFile GetJsonSerializedFile(string Base64File, Dictionary<string, object> FileData)
        {
            try
            {
                object FileType = new object();
                NativeJsonFile NativeJsonFile = new NativeJsonFile();
                bool FileTyeFounded = FileData.TryGetValue("FileType", out FileType);
                if (!FileTyeFounded)
                    throw new Exception(ServicesConstants.NotTypeFileFound);
                
                var clazz = Type.GetType("Hexagon.Services.ConvertSourceFileToJsonStrategy.Convert" + FileType.ToString() + "ToJsonStrategy");
                var Strategy = (IConvertSourceFileToJsonStrategy)Activator.CreateInstance(clazz);
                return Strategy.Do(Base64File, FileData);
            }
            catch (Exception Exception) 
            {
                throw Exception;
            }
        }
    }
}
