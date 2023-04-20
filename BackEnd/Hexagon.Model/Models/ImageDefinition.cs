using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hexagon.Model.Models
{
    public struct ImageDefinition
    {
        public float OriginalMinX { get; }
        public float OriginalMinY { get; }
        public float OriginalMaxX { get; }
        public float OriginalMaxY { get; }

        public float OriginalRangeX { get; }
        public float OriginalRangeY { get; }
        public float HexagonSize { get; set; }
        public float ProportationToScale { get; }
        public float TransformedWidth { get; }
        public float TransformedHeigth { get; }

        public float MaxProportion { get; }
        public Layout Layout { get; }
        public Point Scale { get; set; }

        public ImageDefinition(List<Model.Point> PointsToTransform, Layout Layout)
        {

            this.OriginalMinX = PointsToTransform.Min(x => x.X);
            this.OriginalMaxX = PointsToTransform.Max(x => x.X);
            this.OriginalMinY = PointsToTransform.Min(x => x.Y);
            this.OriginalMaxY = PointsToTransform.Max(x => x.Y);
            this.OriginalRangeX = this.OriginalMaxX - this.OriginalMinX;
            this.OriginalRangeY = this.OriginalMaxY - this.OriginalMinY ;
            this.HexagonSize = 2f;
            this.Scale = new Point(Layout.MaxPictureSizeX, Layout.MaxPictureSizeY);
            if (this.OriginalRangeX > this.OriginalRangeY)
            {
                if (MathF.Floor(Layout.MaxPictureSizeX / Layout.HexPerLine) > this.HexagonSize)
                {
                    this.HexagonSize = MathF.Floor(Layout.MaxPictureSizeX / Layout.HexPerLine);
                }

                this.TransformedWidth = Layout.MaxPictureSizeX;
                this.ProportationToScale = MathF.Floor(TransformedWidth / OriginalRangeX);
                TransformedHeigth = MathF.Ceiling(this.OriginalRangeY * ProportationToScale) ;

            }
            else
            {
                if (MathF.Floor(Layout.MaxPictureSizeY / Layout.HexPerLine) > this.HexagonSize)
                {
                    this.HexagonSize = MathF.Floor(Layout.MaxPictureSizeY / Layout.HexPerLine);
                }
                TransformedHeigth   = Layout.MaxPictureSizeY;
                ProportationToScale = MathF.Floor(TransformedHeigth / (OriginalMaxY - OriginalMinY));
                TransformedWidth = MathF.Ceiling( OriginalRangeX * ProportationToScale);
                
            }
            this.Layout = Layout;
            MaxProportion = OriginalRangeX > OriginalRangeY ? OriginalRangeX : OriginalRangeY;
        }

        public ImageDefinition(List<Model.Point> PointsToTransform, Point Scale = null )
        {
            if (Scale == null)
                this.Scale = new Point(1000, 1000);
            else
                this.Scale = Scale;
            this.OriginalMinX = PointsToTransform.Min(x => x.X);
            this.OriginalMaxX = PointsToTransform.Max(x => x.X);
            this.OriginalMinY = PointsToTransform.Min(x => x.Y);
            this.OriginalMaxY = PointsToTransform.Max(x => x.Y);
            this.OriginalRangeX = this.OriginalMaxX - this.OriginalMinX;
            this.OriginalRangeY = this.OriginalMaxY - this.OriginalMinY;
            
            MaxProportion = OriginalRangeX > OriginalRangeY ? OriginalRangeX : OriginalRangeY;

            if (this.OriginalRangeX > this.OriginalRangeY)
            {

                    this.HexagonSize = 1;
                

                this.TransformedWidth = Scale.X ;
                this.ProportationToScale = MathF.Floor(TransformedWidth / OriginalRangeX);
                TransformedHeigth = MathF.Ceiling(this.OriginalRangeY * ProportationToScale);

            }
            else
            {
                

                    this.HexagonSize = 1;
                
                TransformedHeigth = Scale.Y;
                ProportationToScale = MathF.Floor(TransformedHeigth / OriginalRangeY);
                TransformedWidth = MathF.Ceiling(OriginalRangeX * ProportationToScale);

            }
            this.Layout = new Layout
            {
                FillPolygon = false,
                Flat = true,
                HexPerLine = (int)this.TransformedWidth,
                Origin = new System.Drawing.PointF(0, 0),
                Size = new System.Drawing.PointF(this.HexagonSize, this.HexagonSize)
                
            }
                ;
            


        }
     
    }
    /*def asRadians(degrees):
return degrees * pi / 180

def getXYpos(relativeNullPoint, p):
""" Calculates X and Y distances in meters.
"""
deltaLatitude = p.latitude - relativeNullPoint.latitude
deltaLongitude = p.longitude - relativeNullPoint.longitude
latitudeCircumference = 40075160 * cos(asRadians(relativeNullPoint.latitude))
resultX = deltaLongitude * latitudeCircumference / 360
resultY = deltaLatitude * 40008000 / 360
return resultX, resultY
    */
}

