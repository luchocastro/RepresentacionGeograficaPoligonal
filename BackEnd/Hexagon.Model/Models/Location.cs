using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public struct Location
    {
        public string ProyectFolder { get; }
        public string MapsFolder { get; }
        public string FileFolder { get; }
        public string NativeFileFolder { get; }
        public  Location(string ProyectFolder, string MapsFolder, string FileFolder, string NativeFileFolder)
        {
            this.ProyectFolder = ProyectFolder;
            this.MapsFolder = MapsFolder;
            this.FileFolder = FileFolder;
            this.NativeFileFolder = NativeFileFolder;
        }
    }
}
