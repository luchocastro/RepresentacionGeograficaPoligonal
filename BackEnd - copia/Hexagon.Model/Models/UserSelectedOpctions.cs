using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class UserSelectedOpctions
    {
       public ColorPalette ColorPalette { get; set; }
        public DataInfo LastDataInfo { get; set; }
        public DataFileConfiguration LastDataFileConfiguration { get; set; }
        public AnalizedFile LastAnalizedFile { get; set; }
        public string ActualProyectName{ get; set; }
        public NativeJsonFile LastNativeJsonFile { get; set; }
        public List<string> ProyectNames { get; set; }
        public Function Function { get; set; }
    }
}
