using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Hexagon.IO;
namespace Hexagon.Model.Models
{
    public class HexagonsRow :IPackable 
    {
        public HexagonsRow()         { }
        
        public List<Hex> Hexagons { get; set; } = new List<Hex>();
        
        public List<Point> Points { get; set; } = new List<Point>();
        public  long  Row{ get; set; }   
        public List<System.Drawing.PointF> Corners { get; set; } = new List<System.Drawing.PointF>();
        
        public object FromString(string ObjectPackaged)
        {
            var ret = JsonSerializer.Deserialize<KeyValuePair<long, List<float[]>>>(ObjectPackaged);
            Hexagons.AddRange(ret.Value.Select(x => new Hex(x [0], x[1],x[2]) ));
            
            Row =ret.Key;
            return this;
        }
        public override string ToString()
        {
            return ObjectToString(); 
        }
        public string ObjectToString()
        {

            var ret =  new KeyValuePair<long, List<float[]>>(Row, Hexagons.Select(x => new float[] { x.Q, x.R, x.S }).ToList());
            return JsonSerializer.Serialize(ret);
        }
        public string GetHex()
        {
            var HexString = new List<string>();
            foreach (var item in Hexagons)
            {
                HexString.Add(item.ToString());
            }


            return "";

        }
    }
}
