using Hexagon.Model;
using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    class ConvertRasterSentinelToJsonStrategy : IConvertSourceFileToJsonStrategy
    {
        NativeJsonFile IConvertSourceFileToJsonStrategy.Do(string Base64File, DataFileConfiguration FileData)
        {

            throw new NotImplementedException();
        }

        NativeFile IConvertSourceFileToJsonStrategy.DoFromFile(string PathOrigen, DataFileConfiguration FileData)
        {
            Vintasoft.Imaging.VintasoftImage vsImage = new Vintasoft.Imaging.VintasoftImage(PathOrigen);
            throw new NotImplementedException();
        }
    }
}
