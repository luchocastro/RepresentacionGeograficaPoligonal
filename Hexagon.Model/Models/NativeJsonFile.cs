﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class NativeJsonFile
    {
        public string Content { get; set; }
        public string[] Columns { get; set; }
        public List<Type> ColumnsType { get; set; }
     }
}
