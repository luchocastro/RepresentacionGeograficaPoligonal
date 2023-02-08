using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class MapDefinitionFileDataManager : FileDataManager<MapDefinitionDTO, MapDefinition>
    {
        public MapDefinitionFileDataManager(IMapper Mapper) : base(Mapper)
        {

        }
    }
}
