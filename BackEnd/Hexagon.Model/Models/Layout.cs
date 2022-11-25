using System.Drawing;
using Math = System.MathF;
namespace Hexagon.Model
{
    public struct Layout
    {
        public Layout(bool flat, PointF size, PointF origin)
        {
            if (flat)
                this.Orientation = new Orientation(3.0f / 2.0f, 0.0f, Math.Sqrt(3.0f) / 2.0f, Math.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Math.Sqrt(3.0f) / 3.0f, 0.0f);
            else
                this.Orientation = new Orientation(Math.Sqrt(3.0f), Math.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, Math.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);

            this.Size = size;
            this.Origin = origin;
            this.Flat = flat;
        }
        public Orientation Orientation { get; }
        public PointF Size { get; }
        public PointF Origin { get; }

        public bool Flat { get; }
        

    }
}
