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
                //List<System.Drawing.PointF> PointsSacaled = new List<System.Drawing.PointF>();
                //foreach (var item in Points)
                //{

                //    //PointsSacaled.Add(new System.Drawing.Point(Convert.ToInt32(HexagonFunction.scaleLinear(item.X, 0, width, -180f,  180f)),
                //    //    Convert.ToInt32(HexagonFunction.scaleLinear( item.Y , heigth , 0,  -90f,  90f))));
                //    //                    PointsSacaled.Add(new System.Drawing.PointF(( (item.X - Dif0X) * prop), ( ( maxY- Dif0Y-(item.Y - Dif0Y)) * prop)));
                //    PointsSacaled.Add(new System.Drawing.PointF(item.X  ,item.Y  ));

                //}
                Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; 
                Graphics.DrawPolygon ( Pen, Points.Select(x => new System.Drawing.PointF(MathF.Round(x.X,0), MathF.Round(x.Y))).ToArray());
            }
            //https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/types-of-bitmaps?view=netframeworkdesktop-4.8
            Bitmap.Save(new FileStream (Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp", FileMode.Create )
, ImageFormat.Bmp);

            return Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp";


        }


    }
    
}
