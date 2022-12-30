using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Shared.Enums
{
        public enum EnumActionToDoWithUncastedDTO

        {
            [Description("Eliminar Data")]
            DeleteData = 1,
            [Description("Eliminar Fila")]
            DeleteRow,
            [Description("Detener Proceso")]
            StopProcess
        }
    
}
