using Hexagon.Model.Models;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using Math = System.MathF;
namespace Hexagon.Model
{
    public class Layout :Base
    {
        public Layout()
        { }
        public Layout(bool flat, PointF size, PointF origin,int HexPerLine, float MaxPictureSizeX = 3200f, float MaxPictureSizeY = 3200f, bool FillPolygon = false )
        {
            

            this.Size = size;
            this.Origin = origin;
            this.Flat = flat;
            this.HexPerLine = HexPerLine;
            this.MaxPictureSizeX = MaxPictureSizeX;
            this.MaxPictureSizeY = MaxPictureSizeY;
            this.FillPolygon = FillPolygon;
            this.MapDefinition = MapDefinition;
            this.PaintLines = true;

        }
        public Orientation Orientation { get { return new Orientation(3.0f / 2.0f, 0.0f, Math.Sqrt(3.0f) / 2.0f, Math.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Math.Sqrt(3.0f) / 3.0f, 0.0f); } set {; } } 
        public PointF Size { get; set; }
        public PointF Origin {get; set; }
        public int HexPerLine { get; set; }
        public bool Flat { get; set; }
        public float MaxPictureSizeX { get; set; }
        public float MaxPictureSizeY { get; set; }
        public bool FillPolygon { get; set; }
        public bool PaintLines { get; set; }
        public MapDefinition MapDefinition { get; set; }

        public override string ToString()
        {
            var ToHashed =  this.MaxPictureSizeX.ToString();
            ToHashed += this.MaxPictureSizeY.ToString();
            ToHashed += this.Size.X.ToString();
            
            ToHashed += Convert.ToInt32(this.FillPolygon).ToString();
            ToHashed += Convert.ToInt32(this.PaintLines).ToString();
            
            string ret = HashHelper.sha256Hashed(ToHashed); 
            return ret ;

        }


    }
}
