 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services 
{
    public interface IFileServiceOptions
    {
        public FileServiceOptions Get();
        public static string Position { get { return "FileServiceOptionsPosition"; } }

    }
}
