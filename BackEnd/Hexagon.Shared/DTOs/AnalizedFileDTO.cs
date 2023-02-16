using Hexagon.Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Hexagon.Shared.DTOs
{
    public class AnalizedFileDTO : BaseDto
    {
        public AnalizedFileDTO() 
        {
            NativeFiles = new List<HexFileDTO>();
            MapFiles = new List<HexFileDTO>();
            Files = new List<HexFileDTO>();
        }
        public string PathNatives { get; set; }
        public string PathMaps { get; set; }
        public string PathFiles { get; set; }
        public string NicName { get; set; }
        [JsonIgnore]
        public string OriginalFileName { get; set; }
        [JsonIgnore] 
        public string FileName { get; set; }
        public List<HexFileDTO> NativeFiles { get; set; }
        public List<HexFileDTO> MapFiles { get; set; }
        public List<HexFileDTO> Files { get; set; }
         

    }
}
