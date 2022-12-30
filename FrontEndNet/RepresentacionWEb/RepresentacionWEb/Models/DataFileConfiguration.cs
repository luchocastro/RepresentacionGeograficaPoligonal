using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepresentacionWEb.Models
{
    public class DataFileConfiguration
    {
        public string FileType { get; set; }
        public Dictionary<string, object> FileProperties { get; set; }
    }

}
