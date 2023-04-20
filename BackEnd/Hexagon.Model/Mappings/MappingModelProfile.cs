using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Hexagon.Model;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Hexagon.Shared.Enums;

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
            CreateMap<EnumActionToDoWithUncasted, EnumActionToDoWithUncastedDTO>();
            CreateMap<EnumAlowedDataType , EnumAlowedDataTypeDTO >();
            CreateMap<Line, LineDTO>();
            CreateMap<LineDTO,Line>();
            CreateMap<Column, ColumnDTO>();
            CreateMap<ColumnDTO, Column>();
            CreateMap<NativeFileDTO, NativeFile>();
            CreateMap<NativeFile, NativeFileDTO>(); ;
            CreateMap<Function, FunctionDTO>();
            CreateMap<FunctionDTO, Function>();
            CreateMap<HexagonDetail, HexagonDetailDTO>();
            CreateMap<HexagonDetailDTO , HexagonDetail>();
            CreateMap<HexagonDetails, HexagonDetailsDTO>();
            CreateMap<HexagonDetailsDTO, HexagonDetails>();
            CreateMap<HexagonPosition, HexagonPositionDTO>();
            CreateMap<HexagonPositionDTO, HexagonPosition>();
            CreateMap<CalculatedHexagon, CalculatedHexagonDTO>();
            CreateMap<CalculatedHexagonDTO, CalculatedHexagon>();
            CreateMap<HexaDetailWithValueDTO, HexaDetailWithValue>();
            CreateMap<HexaDetailWithValue, HexaDetailWithValueDTO>();

            CreateMap<OrientationDto, Orientation>();
            CreateMap<Orientation, OrientationDto>();
            CreateMap<PaletteClass, PaletteClassDTO>();
            CreateMap<PaletteClassDTO, PaletteClass>();
            CreateMap<EnumPaletteClass, EnumPaletteClassDTO>();
            CreateMap<EnumPaletteClassDTO, EnumPaletteClass>();
            CreateMap<Point, PointDTO>();
            CreateMap<PointDTO, Point>();
            CreateMap<ImageDefinition, ImageDefinitionDTO>();
            CreateMap<ImageDefinitionDTO, ImageDefinition>();
        }
    }
}
