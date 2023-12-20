using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class DatosMapaPost
    {

         public String HexID { get; set; }

        public int MaxGroupForMetrics { get; set; }
        public int MaxHexsForALine { get; set; }


        public ColumnToPost X { get; set; }
        public ColumnToPost Y { get; set; }


    }
}
