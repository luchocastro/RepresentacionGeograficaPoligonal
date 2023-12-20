using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class Mask
    { 
      public string User { get; set; }
        public string ProyectData { get; set; }
        public string AnalizedFile { get; set; }

        public string HexFile { get; set; }
    
    }
    public class FileDataManagerOptions
    {
        public static string Position { get { return "FileDataManager"; } }
        public Mask Mask { get; set; }
        public string DefaultMask { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public string  DefaultExtension  { get; set; }
        public string ParentDictionaryClass { get; set; }
        
        
    }
    
}
