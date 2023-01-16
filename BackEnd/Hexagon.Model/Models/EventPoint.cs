using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Hexagon.Model
{
    public class EventPoint
    {

        public PointF PositionInMeters { get; set; }
        public PointF PositionInDegrees { get; set; }
        public DateTime EventTime { get; set ; }
        public String Description { get; set; }
        public float Value { get; set; }
        public List<SingleEvent> Values { get; set; }
        public Dictionary<string, object> Filler {get;}
        public List<Point>  GroupPoints { get; set; }
        public string ListValues
        {
            get
            {
                var ret = "EventPoint:{";
                ret += "Description:" + Description + ",EventTime:"
                    ;
                if (EventTime != null)
                    ret += EventTime.ToString("yyyyMMdd hh:mm:ss ffff");
                    ret += ",Values:[";
                if (Values != null)
                    foreach (var item in Values)
                    {
                        ret += item.ListValues + ",";
                    }
                ret += "]}";
                return ret;
            }
        }


    }
}
