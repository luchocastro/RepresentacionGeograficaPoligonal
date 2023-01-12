using System.Drawing;
using Math = System.MathF;
namespace Hexagon.Model
{
    public struct Layout
    {
        public Layout(bool flat, PointF size, PointF origin,int HexPerLine, float MaxPictureSizeX = 3200f, float MaxPictureSizeY = 3200f, bool FillPolygon = false)
        {
            if (flat)
                this.Orientation = new Orientation(3.0f / 2.0f, 0.0f, Math.Sqrt(3.0f) / 2.0f, Math.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Math.Sqrt(3.0f) / 3.0f, 0.0f);
            else
                this.Orientation = new Orientation(Math.Sqrt(3.0f), Math.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, Math.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);

            this.Size = size;
            this.Origin = origin;
            this.Flat = flat;
            this.HexPerLine = HexPerLine;
            this.MaxPictureSizeX = MaxPictureSizeX;
            this.MaxPictureSizeY = MaxPictureSizeY;
            this.FillPolygon = FillPolygon;
        }
        public Orientation Orientation { get; set; }
        public PointF Size { get; set; }
        public PointF Origin {get; set; }
        public int HexPerLine { get; set; }
        public bool Flat { get; set; }
        public float MaxPictureSizeX { get; set; }

        public float MaxPictureSizeY { get; set; }
        public bool FillPolygon { get; set; }
    }
}
