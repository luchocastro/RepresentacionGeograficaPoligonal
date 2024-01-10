using Hexagon.Model;
using Hexagon.Model.FileDataManager;
using Hexagon.Model.Models;
using Hexagon.Model.Repository;
using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    class ConvertRasterSentinelToJsonStrategy : IConvertSourceFileToJsonStrategy
    {
        public IDataRepository<ColumnDTO, Column> ColumnRepository { get; set; }

        public NativeFile DoFromFile(string PathOrigen, DataFileConfiguration FileData, int FirstNRows = 0)
        {
            throw new NotImplementedException();
        }
        public Task<NativeFile> DoFromFileAsync(string PathOrigen, DataFileConfiguration FileData, int FirstNRows = 0, NativeFile NativeFile = null)
        {
            throw new NotImplementedException();
        }
        NativeJsonFile IConvertSourceFileToJsonStrategy.Do(string Base64File, DataFileConfiguration FileData)
        {

            throw new NotImplementedException();
        }
    
     
    }
}
