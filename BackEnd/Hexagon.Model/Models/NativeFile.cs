﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class NativeFile
    {
        [JsonProperty("Content")]
        public List<Line> Content { get; set; }
        public List<Column> Columns { get; set; }
        public string PathFile { get; set; }


    }
}
