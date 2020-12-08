using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
namespace CrearDibujo
{
    public class CrearDibujo
    {
        public static Bitmap DrawPoligons(List<Point []> PoligonsToPaint, Size SizeOfImage)
        {
            MemoryStream MemoryStream = new MemoryStream();
            Bitmap Bitmap = new Bitmap (SizeOfImage.Width,SizeOfImage.Height);
            Graphics Graphics = Graphics.FromImage(Bitmap);
            Pen Pen = new Pen(Brushes.AliceBlue);
            foreach (var PoligonToPaint in PoligonsToPaint)
            {
                Graphics.DrawPolygon(Pen, PoligonToPaint);
            }
            return Bitmap;

        }
    }

}
