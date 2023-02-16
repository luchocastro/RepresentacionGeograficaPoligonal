﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Hexagon.Model;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;

namespace Hexagon.Model.Mappings
{
    public class MappingModelProfile : Profile
    {
        public MappingModelProfile()
        {
            //Log Mappings
            CreateMap<Hexagon.Model.Models.NativeJsonFile, Hexagon.Shared.DTOs.NativeJsonFileDTO>();
            CreateMap<Hexagon.Model.Models.User, Hexagon.Shared.DTOs.UserDTO>();
            CreateMap <Orientation, Shared.DTOs.OrientationDto>();
            
            CreateMap<AnalizedFile, AnalizedFileDTO>();
            CreateMap<AnalizedFileDTO, AnalizedFile>();
            CreateMap<DataFileConfiguration, DataFileConfigurationDTO>();
            CreateMap<DataFileConfigurationDTO, DataFileConfiguration>();
            CreateMap<Layout, LayoutDto>();
            CreateMap<LayoutDto, Layout>();
            CreateMap<ProyectData, ProyectDataDTO>();
            CreateMap<ProyectDataDTO, ProyectData>();
            CreateMap<Location, LocationDTO>();
            CreateMap<LocationDTO,Location>();
            CreateMap<MapDefinitionDTO, MapDefinition>();
            CreateMap<MapDefinition, MapDefinitionDTO>();
            CreateMap<FunctionDTO, Function>();
            CreateMap<Function, FunctionDTO>();
            CreateMap<HexFileDTO , HexFile>();
            CreateMap<HexFile, HexFileDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
