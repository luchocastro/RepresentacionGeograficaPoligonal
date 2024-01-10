using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Hexagon.Model.Models
{
    using Hexagon.IO;
    public class PointsRows : IPackable 
    {
        private List<Point> _Points = new List<Point>();
        private List <long> _Rows = new List<long>  ();





        [ModelSaveAtributes(InPackage = true, PropertyOrder = 0)]
        public List<long> Rows { get { return _Rows; }  }



        public List<Point> Points { get { return _Points ; }set { _Points = value; } }


        public string GetPoints() { return String.Join(",", Points.Select(c => c.ToString()).ToArray()); }

        public string GetRows () { return String.Join(",", Rows.Select(c => c.ToString()).ToArray()); }

        public void AddPoint (Point Point) { 
            if(_Points.Count(x=>x.X == Point.X & x.Y == Point.Y)==0)
                _Points.Add(Point); 

        }

        

        public string ObjectToString()
        {
            
            var ret = new Dictionary<string, string>();

            ret.Add("Points", JsonSerializer.Serialize(Points.Select(x => ((Point)(IPackable)x).ObjectToString())));
            
            return JsonSerializer.Serialize(ret); 

        }


        public object FromString  (string ObjectPackaged)
        {
            var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(ObjectPackaged);
            var ret = new PointsRows();
            var PointsString = JsonSerializer.Deserialize<List<string>>(dic["Points"]);
            
            foreach (var item in PointsString)
            {
                ret.Points.Add((Point)new Point().FromString(item)) ;
            } 
            
            return (PointsRows)this;
        }
    }
}
