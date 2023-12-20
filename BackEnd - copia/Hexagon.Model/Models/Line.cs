using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class Line
    {
        public Line()
        {

            this.Number = 0;
            this.Fieds = null;
        }
        public Line(long Number, string [] Fields)
        {
            this.Number = Number;
            this.Fieds = Fields ;
        }
        [JsonProperty("Fieds")]
        public string []Fieds { set; get; }
        public long Number { set; get; }

    }
}
