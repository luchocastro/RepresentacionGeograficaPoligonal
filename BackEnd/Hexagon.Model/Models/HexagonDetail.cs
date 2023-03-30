using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Hexagon.Model.Models
{
    public class HexagonDetails : Base
    {

        public HexagonDetails() { }
        public HexagonDetails(List<Column> Columns) { this.Columns = Columns; }
        [JsonIgnore]
        public List<HexagonDetail>  List { get;  set; }
        [JsonIgnore]
        public List <Column> Columns { get; set; }
        [JsonIgnore]
        public string Name { get; set; }
         
        public int IndexOfHexagon(HexagonPosition HexagonPosition)
        {
            if (this.List == null || this.List.Count == 0)
                return -1;
            return this.List.Select(x => x.IndexOfHexagonPosition(HexagonPosition)).First();
        }
        public void Add (HexagonDetail HexagonDetail)
        {
            if (this.List == null)
                this.List = new List<HexagonDetail>();
            this.List.Add(HexagonDetail);
        }
    }
    public struct HexagonPosition
    {
        public float Q { get; set; }
        public float R { get; set; }
        public float S { get; set; }
        
    }
    public class HexagonDetail  
    {
        public HexagonDetail()
        {
            this.HexagonPositionForValues = new List<HexagonPosition>();
            this.Lines = new List<Line>();
            
        }
        [JsonIgnore]
        public float Value { get; set; }
        [JsonIgnore]
        public string Color
        {
            get; set;
        }
        public List<long> IndexLines
        {
            get; set;
        } = new List<long>();
        public int[] RGBColor
        {
            get; set;
        }
        [JsonIgnore]
        public float Opacity { get; set; }
        [JsonIgnore] 
        public string BorderColor { get; set; }
        [JsonIgnore] 
        public string BorderType { get; set; }
        public List<Line> Lines { get; set; }
        public List<HexagonPosition> HexagonPositionForValues { get; set; }
        public int IndexOfHexagonPosition(HexagonPosition HexagonPosition)
        {
            return HexagonPositionForValues.FindLastIndex(x => x.Q == HexagonPosition.Q && x.R == HexagonPosition.R && x.S == HexagonPosition.S);
        }
    }
}
