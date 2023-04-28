using Hexagon.Model.Repository;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexagon.Model.Models
{
    public class ProyectData : Equatable<ProyectData>  
    {
        public Location Location { get; set; }
        [JsonIgnore]
        public List<AnalizedFile> AnalizedFiles { get; set; }

        
    }
}
