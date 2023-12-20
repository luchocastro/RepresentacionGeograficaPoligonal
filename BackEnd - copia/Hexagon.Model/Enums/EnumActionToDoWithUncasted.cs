using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hexagon.Model
{
        public enum EnumActionToDoWithUncasted
        {
            [Description("Eliminar Data")]
            DeleteData = 1,
            [Description("Eliminar Fila")]
            DeleteRow,
            [Description("Detener Proceso")]
            StopProcess
        }
    
}
