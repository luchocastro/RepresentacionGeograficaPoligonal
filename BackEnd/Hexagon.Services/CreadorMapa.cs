using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Hexagon.Services
{
    public class CreadorMapaService
    {
        public HexagonGrid HexagonGrid = new HexagonGrid();
        public static string CreadorMapa(List<Hex> Hexs, string PathFile, Layout layout)
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
             MemoryStream MemoryStream = new MemoryStream();
             Bitmap Bitmap = new Bitmap((int)layout.MaxPictureSizeX, (int)layout.MaxPictureSizeY);
            Graphics Graphics = Graphics.FromImage(Bitmap);
            ;
            foreach (var hexagon in Hexs )
            {
                Color color = Color.FromArgb(hexagon.RGBColor[0], hexagon.RGBColor[1], hexagon.RGBColor[2]);
                Pen Pen = new Pen(color);
                
                var Points = Hexagon.Services.Helpers.HexagonFunction.GetPoints(hexagon, layout);
                if (Points.Where(x => x.X < 0).Count() > 0 || Points.Where(x => x.Y < 0).Count() > 0)
                    throw new ArgumentOutOfRangeException();
                Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                Graphics.DrawPolygon ( Pen, Points.Select(x => new System.Drawing.PointF(MathF.Round(x.X , 0) , MathF.Round(x.Y))).ToArray());

            }
            Bitmap.Save(new FileStream (Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp", FileMode.Create )
, ImageFormat.Bmp);

            return Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp";


        }


    }
    
}
