using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    public interface IFileService
    {
        public NativeJsonFileDTO ConvertFile(string Base64File, DataFileConfigurationDTO FileData);
        public List<DataFileConfigurationDTO> GetDataFileConfiguration();


    }
}
