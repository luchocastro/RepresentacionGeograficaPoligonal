using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
     public struct AnalizedFile : IModelPersistible
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string NicName { get; set; }
        public string ID { get; set; }

    }
}
