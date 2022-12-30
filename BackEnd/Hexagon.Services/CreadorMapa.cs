using System;
using System.Collections.Generic;
using Hexagon.Shared;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Hexagon.Model;
using GeoJSON.Net;
using Hexagon.Services.Helpers;
using System.Linq;
using BitMiracle.Jpeg2k;

namespace Hexagon.Services
{
    public class CreadorMapaService
    {
        public HexagonGrid HexagonGrid = new HexagonGrid();
        public static string CreadorMapa(List<Hex> Hexs, string PathFile)
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
            List<System.Drawing.PointF[]> PoligonsToPaint = null;
            Size SizeOfImage = new Size();
            //Vintasoft.Imaging.VintasoftImage vsImage = new Vintasoft.Imaging.VintasoftImage(Path.GetDirectoryName(PathFile) + @"prueba.jp2");
            //var J2kImage = CSJ2K.J2kImage.FromFile ( Path.GetDirectoryName(PathFile) + @"prueba.jp2"));
            //var J2kImageFromStream = CSJ2K.J2kImage.FromStream(new StreamReader(Path.GetDirectoryName(PathFile) + @"prueba.jp2").BaseStream);
            MemoryStream MemoryStream = new MemoryStream();

            using (var image = new StreamReader(Path.GetDirectoryName(PathFile) + @"Pruebadd.tif"))
            {
                var text = image.ReadToEnd();
            }
             

            Layout layout = new Layout(true, new System.Drawing.PointF(0.1f, 0.1f), new System.Drawing.PointF(0, 0));
            var PointsAll  = (Hexs.Select(x => Hexagon.Services.Helpers.HexagonFunction.GetPoints(x, layout))  ) ;
            List<System.Drawing.PointF> PointsList = new List<System.Drawing.PointF>();

            foreach (var item in PointsAll)
            {
                PointsList.AddRange(item);
            } 
            var minX =PointsList.Select(x => x.X).Min();
            var maxX = PointsList.Select(x => x.X).Max();
            var minY= PointsList.Select(x => x.Y).Min();
            var maxY = PointsList.Select(x => x.Y).Max();
            var width = 3200; //Convert.ToInt32 ( HexagonFunction.scaleLinear (1600f, 0,1600f, 0f , minX,maxY));
            var heigth = 3200; // Convert.ToInt32(HexagonFunction.scaleLinear(maxY, maxY - minY, 0, 0, 1600)); ;
            Bitmap Bitmap = new Bitmap(width , heigth );
            Graphics Graphics = Graphics.FromImage(Bitmap);
            ;
            foreach (var hexagon in Hexs)
            {
                Color color = Color.FromArgb( 0,  hexagon.RGBColor[1], hexagon.RGBColor[2]);
                Pen Pen = new Pen(color);
                
                var Points = Hexagon.Services.Helpers.HexagonFunction.GetPoints(hexagon, layout);
                List<System.Drawing.Point> PointsSacaled = new List<System.Drawing.Point>();
                foreach (var item in Points)
                {

                    PointsSacaled.Add(new System.Drawing.Point(Convert.ToInt32(HexagonFunction.scaleLinear(item.X, 0, width, -180f,  180f)),
                        Convert.ToInt32(HexagonFunction.scaleLinear( item.Y , heigth , 0,  -90f,  90f))));
                }
                Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; 
                Graphics.DrawPolygon ( Pen, PointsSacaled.ToArray<System.Drawing.Point>());
            }
            Bitmap.Save(new FileStream (Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp", FileMode.Create )
, ImageFormat.Bmp);
            Bitmap.Save(MemoryStream, ImageFormat.Bmp);
            var imageBytes = MemoryStream.ToArray();


            return Convert.ToBase64String(imageBytes);


        }


    }
    
}
