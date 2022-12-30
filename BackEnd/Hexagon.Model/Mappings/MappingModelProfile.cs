using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Hexagon.Model;
namespace Hexagon.Model.Mappings
{
    public class MappingModelProfile : Profile
    {
        public MappingModelProfile()
        {
            //Log Mappings
            CreateMap<Hexagon.Model.Models.NativeJsonFile, Hexagon.Shared.DTOs.NativeJsonFileDTO>();
            CreateMap<Hexagon.Model.Models.User, Hexagon.Shared.DTOs.UserDTO>();
            CreateMap <Model.Orientation, Shared.DTOs.OrientationDto>();
            CreateMap<Model.Layout, Shared.DTOs.LayoutDto>();

        }
    }
}
