using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public struct Location
    {
        public string ProyectFolder { get; set; }
        public string MapsFolder { get; set; }
        public string FileFolder { get; set; }
        public string NativeFileFolder { get; set; }
        public  Location(string ProyectFolder, string MapsFolder, string FileFolder, string NativeFileFolder)
        {
            this.ProyectFolder = ProyectFolder;
            this.MapsFolder = MapsFolder;
            this.FileFolder = FileFolder;
            this.NativeFileFolder = NativeFileFolder;
        }
    }
}
