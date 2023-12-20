using System;
using System.Collections.Generic;
using Math = System.MathF;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public struct OrientationDto
    {
        public OrientationDto(float f0, float f1, float f2, float f3, float b0, float b1, float b2, float b3, float startAngle)
        {
            this.F0 = f0;
            this.F1 = f1;
            this.F2 = f2;
            this.F3 = f3;
            this.B0 = b0;
            this.B1 = b1;
            this.B2 = b2;
            this.B3 = b3;
            this.StartAngle = startAngle;
        }
        public float F0 { get; }
        public float F1 { get; }
        public float F2 { get; }
        public float F3 { get; }
        public float B0 { get; }
        public float B1 { get; }
        public float B2 { get; }
        public float B3 { get; }
        public float StartAngle { get; }

    }
}
