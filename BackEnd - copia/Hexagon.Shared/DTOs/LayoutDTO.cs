﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Math = System.MathF;
using System.Text.Json.Serialization;
using Hexagon.Shared.DTOs.Base;

namespace Hexagon.Shared.DTOs
{
    public class LayoutDto : BaseDto
    {
        public LayoutDto()
        { }
         

        public OrientationDto Orientation { get; set; }  
        public PointF Size { get; set; }
        public PointF Origin { get; set; }
        public bool Flat { get; set; }
        public int HexPerLine { get; set; }
        public float MaxPictureSizeX { get; set; }

        public float MaxPictureSizeY { get; set; }
        public bool  FillPolygon { get; set; }
        public MapDefinitionDTO MapDefinition { get; set; }

    }
}
