using System;
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
            public LayoutDto(bool flat, PointF size, PointF origin,int HexPerLine, float MaxPictureSizeX= 3200f, float MaxPictureSizeY=3200f, bool FillPolygon = false )
        {
            if (flat)
                this.Orientation = new OrientationDto(3.0f / 2.0f, 0.0f, Math.Sqrt(3.0f) / 2.0f, Math.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Math.Sqrt(3.0f) / 3.0f, 0.0f);
            else
                this.Orientation = new OrientationDto(Math.Sqrt(3.0f), Math.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, Math.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);

            this.Size = size;
            this.Origin = origin;
            this.Flat = flat;
            this.MaxPictureSizeX = MaxPictureSizeX;
            this.MaxPictureSizeY = MaxPictureSizeY;
            this.HexPerLine = HexPerLine;
            this.FillPolygon = FillPolygon;
        }

        public OrientationDto Orientation { get; set; }  
        public PointF Size { get; set; }
        public PointF Origin { get; set; }
        public bool Flat { get; set; }
        public int HexPerLine { get; set; }
        public float MaxPictureSizeX { get; set; }

        public float MaxPictureSizeY { get; set; }
        public bool  FillPolygon { get; set; }
        public MapDefinitionDTO MapDefinition { get; set; }
        public string Name { get; set; }

    }
}

