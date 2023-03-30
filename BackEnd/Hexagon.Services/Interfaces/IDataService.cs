using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    interface IDataService
    {
        Column VerifyData(string HexFileID, string ColumnName, EnumAlowedDataType DataType, string Mask = "");

    }
}
