using System;
using System.Collections.Generic;
using System.IO;
using Hexagon.Model;
using Hexagon.Model.Models;
namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    public interface IConvertSourceFileToJsonStrategy
    {
        NativeJsonFile Do(string Base64File, DataFileConfiguration FileData);
    }
}
