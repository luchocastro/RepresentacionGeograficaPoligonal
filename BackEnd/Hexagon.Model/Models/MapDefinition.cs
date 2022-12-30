using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public struct MapDefinition
    {
        public Layout Layout { get; set; }
        public string ColumnNameForX { get; set; }
        public string ColumnNameForY { get; set; }
        public string ColumnForMapGroup { get; set; }
        public List<string> ColumnsNameForFuntion { get; set; }
        public string FunctionName { get; set; }
        public EnumActionToDoWithUncasted ActionToDoWithUncasted { get; set; }
        public MapDefinition(
        Layout _Layout,
        string _ColumnNameForX,
        string _ColumnNameForY,
        string _ColumnForMapGroup,
        List<string> _ColumnsNameForFuntion,
        string _FunctionName,
        EnumActionToDoWithUncasted _ActionToDoWithUncasted)
        {
            this.Layout = _Layout;
            this.ColumnNameForX = _ColumnNameForX;
            this.ColumnNameForY = _ColumnNameForY;
            this.ColumnsNameForFuntion = _ColumnsNameForFuntion;
            this.ColumnForMapGroup = _ColumnForMapGroup;
            this.FunctionName = _FunctionName;
            ActionToDoWithUncasted = _ActionToDoWithUncasted;
        }

    }
}
