using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class NativeFile : Base
    {
        private bool IsPolygonDef = false;
        [JsonProperty("Content")]
        public List<Line> Content { get; set; } = new List<Line>();
        public List<Column> Columns { get; set; } = new List<Column>();
        public string PathFile { get; set; }
        public DataFileConfiguration DataFileConfiguration { get; set; } = null;
        public string FileName { get; set; }
        public bool IsPolygon { get; set; } = false;


    }
}
