using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public struct LocationDTO
    {
        public string ProyectFolder { get; }
        public string MapsFolder { get; }
        public string FileFolder { get; }
        public string NativeFileFolder { get; }
        public LocationDTO(string ProyectFolder, string MapsFolder, string FileFolder, string NativeFileFolder)
        {
            this.ProyectFolder = ProyectFolder;
            this.MapsFolder = MapsFolder;
            this.FileFolder = FileFolder;
            this.NativeFileFolder = NativeFileFolder;
        }
    }
}
