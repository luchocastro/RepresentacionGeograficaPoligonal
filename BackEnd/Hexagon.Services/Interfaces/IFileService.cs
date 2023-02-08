﻿using Hexagon.Model;
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
        public NativeFileDTO ConvertFile( DataFileConfigurationDTO FileData, LayoutDto Layout, ProyectDataDTO ProyectDataDTO, string NicData);
        public List<DataFileConfigurationDTO> GetDataFileConfiguration(string Path);
        List<ColumnDTO> GetFileColumsFromFile(string PathDef);
        List<ProyectDataDTO> GetProyects(string User, string Project = "", AnalizedFileDTO AnalizedFileDTO = null); 
        public string GenerateImge(LayoutDto layout, string PathFile);
        public AnalizedFileDTO ConvertFileToHexList(ProyectDataDTO ProyectDataDTO, AnalizedFileDTO AnalizedFileDTO, LayoutDto LayoutDto);


    }
}
