using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
 using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class DataFileConfigurationFileDataManager<G, T> : FileDataManager<G, T> where T : DataFileConfiguration where G : DataFileConfigurationDTO
    {

        public DataFileConfigurationFileDataManager(IMapper Mapper, IConfiguration IConfiguration, IFileDataManagerOptions IFileDataManagerOptions) : base(Mapper, IConfiguration, IFileDataManagerOptions)
        {
             
        }
        public override G Get(string FileType)
        {
            G DataFileConfigurationDTO;
            try
            {
                T DataFileConfigurationToGet = (T) new DataFileConfiguration() ;
                DataFileConfigurationToGet.FileType = FileType;
                DataFileConfigurationToGet.ParentID = Path.Combine(DataFileConfigurationToGet.GetType().Name );
                DataFileConfigurationDTO = base.Get(GenerateFullID(DataFileConfigurationToGet));
                
            }
            catch (Exception ex)
            {
                if ((ex.GetType()) == typeof(FileNotFoundException))
                {
                    return null;
                }
                else
                    throw ex;

            }

            return DataFileConfigurationDTO;

        }
        public override G Add(T DataFileConfiguration)
        {
            DataFileConfiguration.ID = DataFileConfiguration.FileType;
            DataFileConfiguration.ParentID = DataFileConfiguration.GetType().Name ;
            return base.Add(DataFileConfiguration);

        }
    }
}
