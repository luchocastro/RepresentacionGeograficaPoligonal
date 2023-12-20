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
        public List<SingleEvent> Values { get {
                Values = null;
                if (Values == null && 1==2)
                {
                    Values = new List<SingleEvent>();
                    if (this.GroupPoints != null)
                    {
                        foreach (var Point in this.GroupPoints)
                        {
                            var SingleEvent = new SingleEvent();
                            SingleEvent.DateTime = this.EventTime;
                            SingleEvent.Point = Point;
                            SingleEvent.values = (new string[] { this.Value.ToString() }).ToList();
                            Values.Add(SingleEvent);
                        }
                    }
                }
                return Values;
            }
            set { this.Values = value; }
        }
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
