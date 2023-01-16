using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Linq;

namespace Hexagon.Model
{
    public class HexagonGrid: System.Collections.IEnumerable
    {
        private Hex[] _Hexagonos;
        private List<EventPoint> _PuntosACalcular;
        private HexagonGrid _GrillaAnterior ;
        private Function _Function;
        List<float> Valores = null;
        public HexagonGrid(List<EventPoint> PuntosACalcular, Layout  Layout, Function Function)
        {
            this.Layout = Layout;
            this.Function = Function;
            this.PuntosACalcular = PuntosACalcular;
            
        }
        public HexagonGrid()
        {

        }
        public HexagonGrid(HexagonGrid GrillaAnterior, Layout Layout, Function Function)
        {
            this.Layout = Layout;
            
            this.Function = Function;
            this.PuntosACalcular = GrillaAnterior.PuntosACalcular;
        }

        
        public List<Hex> HexagonMap { get; set; }
        public Layout Layout { get;  set; }
        public List<EventPoint> PuntosACalcular { get; set; }
        public Function Function { get; set; }
        
        public IEnumerator GetEnumerator()
        {
            return HexagonMap.GetEnumerator();
        }

    }
}
