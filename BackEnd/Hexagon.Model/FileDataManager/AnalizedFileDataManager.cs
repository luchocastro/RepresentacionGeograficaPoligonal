using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class AnalizedFileDataManager<G, T> : FileDataManager<G, T>  where T : AnalizedFile where G : ProyectDataDTO
    {
        public AnalizedFileDataManager(IMapper Mapper, IConfiguration IConfiguration,
                    IFileDataManagerOptions IFileDataManagerOptions) : base(Mapper, IConfiguration, IFileDataManagerOptions)
        {


        }
    }

}
