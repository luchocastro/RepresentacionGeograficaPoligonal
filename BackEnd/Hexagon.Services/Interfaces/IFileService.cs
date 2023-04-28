using Hexagon.Model;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Services.Interfaces
{
    public interface IFileService
    {
        public NativeJsonFileDTO ConvertFileBase64(string Base64File, DataFileConfigurationDTO FileData);
        public NativeFileDTO ConvertFile(string AbsolutePathFile, DataFileConfigurationDTO FileData, LayoutDto Layout);
        public NativeFileDTO ConvertFile(DataFileConfigurationDTO DataFileConfiguration, string HexFileID);
        public List<DataFileConfigurationDTO> GetDataFileConfiguration(string Path);
        NativeFileDTO GetFileColumsFromFile(DataFileConfigurationDTO DataFileConfiguration, string HexFileID, int FistNRows);
        List<ProyectDataDTO> GetProyects(string User );
        public ProyectDataDTO GetProyect(string User, string ProjeFileDatact = "" );
        public string GenerateImge(string PaletteClassID, string FuctionID, float Size = 0);
        public AnalizedFileDTO ConvertFileToHexList(ProyectDataDTO ProyectDataDTO, AnalizedFileDTO AnalizedFileDTO, LayoutDto LayoutDto);
        public HexFileDTO PutFile(string User, string Project  , string NicName, string FileName, string OriginalName);
        public HexFileDTO PutFile(string User, string Project, string NicName, IFormFile IFormFile);
        List<HexFileDTO> GetHexFiles(string AnalizedFileID);
        List<AnalizedFileDTO> GetAnalizedFiles(string ProyectID);
        public FunctionDTO SetFunction(string HexagonDetailstID, FunctionDTO Function);
        public CalculatedHexagonDTO DoCalc(string FunctionID, List<string> Columns = null);
        List<PaletteClass> GetPaletteClasses(string Name, string Enum, int Q);
        public string GenerateLayout(LayoutDto layout, string PathFile);
        public  HexFileDTO PutFileAsync(string User, string Project, string NicName, string FileName, string OriginalName);
        public Task<HexFileDTO> PutFileAsync(string User, string Project, string NicName, IFormFile IFormFile);
        public Task<NativeFileDTO> ConvertFileAsync(DataFileConfigurationDTO FileData, string HexFileID);


    }
}
    