using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hexagon.Model.Models
{
    public class SingleEvent
    {
        [JsonIgnore]
        public List <string> values { get; set; }
        [JsonIgnore]
        public string ListValues
        {
            get
            {
                string ret = "";
                ret += "SingleEvent:{" +
                    "DateTime:";
                if (DateTime != null)
                    ret += DateTime.ToString("yyyyMMdd hh:mm:ss ffff");
                ret += ";  Point: ";
                if (Point != null)
                    ret += (Point.X.ToString() + ":" + Point.Y.ToString());
                  ret += ";Values:[";
                if (values!=null)
                    foreach (var value in values)
                    {
                        ret += value + ",";
                    }
                ret += "]}";
                return ret;
            }
        } 
        public Point Point { get; set; }
        [JsonIgnore]
        public DateTime DateTime{ get; set; }


    }
}
