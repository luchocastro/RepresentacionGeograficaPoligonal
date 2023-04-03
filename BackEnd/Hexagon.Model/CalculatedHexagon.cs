﻿using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public struct HexaDetailWithValue
    {
        public HexagonPosition HexagonPosition { get;set;}
        public float Value { get; set; }
    }
    public class CalculatedHexagon :Base
    {
        public string Name { get; set; }
        public List<string> ColumnNamesForFunction { get; set; }
        public List<HexaDetailWithValue> HexaDetailWithValue { get; set; }
    }
}