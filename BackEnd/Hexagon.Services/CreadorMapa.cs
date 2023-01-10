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

            //Vintasoft.Imaging.VintasoftImage vsImage = new Vintasoft.Imaging.VintasoftImage(Path.GetDirectoryName(PathFile) + @"prueba.jp2");
            //var J2kImage = CSJ2K.J2kImage.FromFile ( Path.GetDirectoryName(PathFile) + @"prueba.jp2"));
            //var J2kImageFromStream = CSJ2K.J2kImage.FromStream(new StreamReader(Path.GetDirectoryName(PathFile) + @"prueba.jp2").BaseStream);
            MemoryStream MemoryStream = new MemoryStream();

            //using (var image = new StreamReader (Path.GetDirectoryName(PathFile) + @"T20HQE_20221229T134709_B02_10m.jp2.tif"))
            //{
            //    var test = image.ReadLine ();
            //    var decoder = System.Drawing.Imaging.Metafile.FromStream(image.BaseStream); //  . TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //    var enconder = System.Drawing.Imaging.Encoder.Transformation;

            //    var df = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders ();
            //    var bitmapSource = decoder.[0];

            //    // Draw the Image.

            //}

            //byte[] leer = null;
            //using (var image = new FileStream (Path.GetDirectoryName(PathFile) + @"T20HQE_20221229T134709_B02_10m.jp2.tif", FileMode.Open))
            //{

            //    var binary = new BinaryReader(image);
            //    leer = binary.ReadBytes((int)image.Length);
            //    var TifFile = new System.Drawing.ImageConverter();
               

            //}   
            //using (var archivoAtexto = new StreamWriter(Path.GetDirectoryName(PathFile) + @"T20HQE_20221229T134709_B02_10m.txt"))
            //{
            //    var asciiFile = System.Text.UTF8Encoding.UTF8 ;

            //    archivoAtexto.Write(asciiFile.GetString(leer));
            //    archivoAtexto.Flush();
            //    archivoAtexto.Close();
            //}
            
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
            var Dif0Y =  minY;
            var Dif0X =  minX; 
            var prop = 0f;
            var width = 3200f; //Convert.ToInt32 ( HexagonFunction.scaleLinear (1600f, 0,1600f, 0f , minX,maxY));
            var heigth = 3200f; // Convert.ToInt32(HexagonFunction.scaleLinear(maxY, maxY - minY, 0, 0, 1600)); ;
            var mayor = 0f;
            if ( maxX - Dif0X > (maxY - Dif0Y  ))
            {
               //if ((maxX - Dif0X) < width)
                {
                    prop = width / (maxX - Dif0X);
                    heigth = ((maxY - Dif0Y)) * prop;
                    mayor = width;
                }
                //else
                //{
                //    prop = (maxX - Dif0X) / width  ;
                //    heigth = ((maxY - Dif0Y)) * prop;
                //    mayor = width;
                //}
                
            }
            else
            {
                //if ((maxY - Dif0Y) < heigth )
                //{
                    prop = heigth  / (maxY - Dif0Y);
                    width = (maxX - Dif0X) * prop;
                    mayor = heigth;
                //}
                //else
                //{
                //    prop = (maxY - Dif0Y) / heigth;
                //    width = (maxX - Dif0X) * prop;
                //    mayor = heigth;
                //}
            }


            Bitmap Bitmap = new Bitmap((int)MathF.Ceiling ( width) , (int)MathF.Ceiling(heigth ));
            Graphics Graphics = Graphics.FromImage(Bitmap);
            ;
            foreach (var hexagon in Hexs )
            {
                Color color = Color.FromArgb( 135,  254, hexagon.RGBColor[2]);
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
                Graphics.DrawPolygon ( Pen, Points);
            }
            //https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/types-of-bitmaps?view=netframeworkdesktop-4.8
            Bitmap.Save(new FileStream (Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp", FileMode.Create )
, ImageFormat.Bmp);
            Bitmap.Save(MemoryStream, ImageFormat.Bmp);
            var imageBytes = MemoryStream.ToArray();


            return Path.GetDirectoryName(PathFile) + @"\image" + Path.GetFileNameWithoutExtension(PathFile) + ".bmp";


        }


    }
    
}
