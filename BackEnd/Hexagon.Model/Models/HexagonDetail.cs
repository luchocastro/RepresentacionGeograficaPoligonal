using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class HexagonDetails : Base
    {
        public HexagonDetails() { }
        public HexagonDetails(List<Column> Columns) { this.Columns = Columns; }
        public List<HexagonDetail>  List { get;  set; }
        public List <Column> Columns { get; set; }

        public void Add (HexagonDetail HexagonDetail)
        {
            if (this.List == null)
                this.List = new List<HexagonDetail>();
            this.List.Add(HexagonDetail);
        }
    }
    public class HexagonDetail  
    {
        public HexagonDetail(Hex Hex, List<Line> Lines)
        {
            this.Lines = Lines;
            this.Q = Hex.Q;
            this.R = Hex.R;
            this.S = Hex.S;
        }
        public float Q { get; set; }
        public float R { get; set; }
        public float S { get; set; }
        public float Value { get; set; }
     
        public string Color
        {
            get; set;
        }

        public int[] RGBColor
        {
            get; set;
        }
        public float Opacity { get; set; }
        public string BorderColor { get; set; }
        public string BorderType { get; set; }
        public List<Line> Lines { get; set; }
    }
}
