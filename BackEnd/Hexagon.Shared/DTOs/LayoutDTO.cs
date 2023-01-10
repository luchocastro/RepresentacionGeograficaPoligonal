﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Math = System.MathF;
using System.Text.Json.Serialization;

namespace Hexagon.Shared.DTOs
{
    public class LayoutDto
    {
        public LayoutDto(bool flat, PointF size, PointF origin,int HexPerLine)
        {
            if (flat)
                this.Orientation = new OrientationDto(3.0f / 2.0f, 0.0f, Math.Sqrt(3.0f) / 2.0f, Math.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Math.Sqrt(3.0f) / 3.0f, 0.0f);
            else
                this.Orientation = new OrientationDto(Math.Sqrt(3.0f), Math.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, Math.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);

            this.Size = size;
            this.Origin = origin;
            this.Flat = flat;

            this.HexPerLine = HexPerLine;
        }
        public OrientationDto Orientation { get; set; }
        public PointF Size { get; set; }
        public PointF Origin { get; set; }
        public bool Flat { get; set; }
        public int HexPerLine { get; set; }

    }
}
