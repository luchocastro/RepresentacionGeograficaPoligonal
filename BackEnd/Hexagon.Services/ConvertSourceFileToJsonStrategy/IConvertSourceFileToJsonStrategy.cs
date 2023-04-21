using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Hexagon.Model;
using Hexagon.Model.Models;
namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    public interface IConvertSourceFileToJsonStrategy
    {
        NativeJsonFile Do(string Base64File, DataFileConfiguration FileData);
        NativeFile DoFromFile(string PathOrigen, DataFileConfiguration FileData, int FirstNRows=0);
        Task<NativeFile> DoFromFileAsync(string PathOrigen, DataFileConfiguration FileData, int FirstNRows = 0, AnalizedFile AnalizedFile);


    }
}
