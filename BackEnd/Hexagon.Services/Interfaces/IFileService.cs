using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    public interface IFileService
    {
        public NativeJsonFileDTO ConvertFileBase64(string Base64File, DataFileConfigurationDTO FileData);
        public NativeFileDTO ConvertFile(string AbsolutePathFile, DataFileConfigurationDTO FileData, LayoutDto Layout);
        public List<DataFileConfigurationDTO> GetDataFileConfiguration(string Path);
        List<ColumnDTO> GetFileColumsFromFile(string PathDef);
        List<ProyectDataDTO> GetProyects(string User, string Project = "");
        public string GenerateImge(LayoutDto layout, string PathFile);
    }
}
