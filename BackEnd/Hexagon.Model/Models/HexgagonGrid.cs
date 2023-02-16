using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Linq;
using Hexagon.Model.Models;

namespace Hexagon.Model
{
    public class HexagonGrid: System.Collections.IEnumerable, IModelPersistible
    {
        private List<EventPoint> _PuntosACalcular = new List<EventPoint>();
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
            this.PuntosACalcular = GrillaAnterior.HexagonMap.SelectMany(x=>x.Values).ToList();
        }

        
        public List<Hex> HexagonMap { get; set; }
        public Layout Layout { get;  set; }
        public List<EventPoint> PuntosACalcular { get { return _PuntosACalcular; } set { _PuntosACalcular = value; } }
        public Function Function { get; set; }
        public string ID { get ; set ; }

        public IEnumerator GetEnumerator()
        {
            return HexagonMap.GetEnumerator();
        }
        public void GridToFile()
        {


        }

    }
}
