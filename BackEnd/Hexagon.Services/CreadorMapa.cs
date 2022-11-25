using System;
using System.Collections.Generic;
using Hexagon.Shared;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Hexagon.Model;
namespace Hexagon.Services
{
    public class CreadorMapaService
    {
        public string CreadorMapa(string idSesion)
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
            List<System.Drawing.Point[]> PoligonsToPaint = null;
            Size SizeOfImage = new Size();

            MemoryStream MemoryStream = new MemoryStream();
            Bitmap Bitmap = new Bitmap(SizeOfImage.Width, SizeOfImage.Height);
            Graphics Graphics = Graphics.FromImage(Bitmap);


            foreach (var hexagon in HexagonGrid.HexagonMap)
            {
                Color color = Color.FromArgb((byte)hexagon.RGBColor[1], (byte)hexagon.RGBColor[2], (byte)hexagon.RGBColor[3]);
                Pen Pen = new Pen(color);
                Graphics.DrawPolygon(Pen, Hexagon.Services.Helpers.HexagonFunction.GetPoints(hexagon, HexagonGrid.Layout));
            }
            Bitmap.Save(MemoryStream, ImageFormat.Bmp);
            var imageBytes = MemoryStream.ToArray();


            return Convert.ToBase64String(imageBytes);


        }


    }
    
}
