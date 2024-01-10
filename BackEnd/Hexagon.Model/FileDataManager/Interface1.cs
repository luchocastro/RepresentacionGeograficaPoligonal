using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public interface IFileDataManagerOptions
    {
        public FileDataManagerOptions Get();
        public static string Position { get { return "DataManagerPosition"; } }     
        //public Mask Mask { get;  }

    }
}
