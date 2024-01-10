using Hexagon.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class ColumnToPost
    {

        public EnumAlowedDataTypeDTO DataTypeSelected { get; set; }

        public EnumActionToDoWithUncastedDTO ActionToDoWithUncasted { get; set; }
   
        public string Name { get; set; }

        public string Mask { get; set; }
    }
}
