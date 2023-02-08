using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class ProyectData :IModelPersistible 
    {
        public string Name { get; set; }
        public Location Location { get; set; }
        public List<AnalizedFile> AnalizedFiles { get; set; }
        public string ID { get ; set ; }
    }
}
