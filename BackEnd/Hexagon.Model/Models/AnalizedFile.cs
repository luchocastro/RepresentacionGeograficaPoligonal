using Hexagon.Model.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class AnalizedFile :  Equatable<AnalizedFile>  
    {
        public AnalizedFile()
        {
            Files = new List<HexFile>();
            NativeFiles = new List<HexFile>();
           MapFiles = new List<HexFile>();
        }
        public string PathNatives   { get; set; }
        public string PathMaps  { get; set; }
        public string PathFiles { get; set; }

        public string NicName { get; set; }
        [JsonIgnore]
        public List<HexFile> NativeFiles { get; set; }
        [JsonIgnore]
        public List<HexFile> MapFiles { get; set; }
        [JsonIgnore]
        public List<HexFile>  Files { get; set; }
    }
}

