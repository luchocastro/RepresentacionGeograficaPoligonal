using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers
{
    public class PreparedFileDataPost
    {
        public string ColumnX { get; set; }
        public string ColumnY { get; set; }
        public string HexID { get; set; }
        public string[] ListData { get; set; }
    }
}
