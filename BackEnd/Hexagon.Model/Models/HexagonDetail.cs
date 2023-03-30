using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Hexagon.Model.Models
{
    public class HexagonDetails : Base
    {
        Base Base = null;
        public HexagonDetails() : base()
        {
            
        }
        public List<HexagonDetail>  List { get;  set; }
        public string Name { get;  set; } 
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
            
        }
        public List<long> IndexLines
        {
            get; set;
        } = new List<long>();

        public List<HexagonPosition> HexagonPositionForValues { get; set; } = new List<HexagonPosition>();
    }
}
