using System;
using System.Collections.Generic;
using System.Globalization;
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

        public float TransformedMinX { get; }
        public float TransformedMinY { get; }
        public float TransformedMaxX { get; }
        public float TransformedMaxY { get; }

        public float TransformedRangeX { get; }
        public float TransformedRangeY { get; }
        
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
                this.ProportationToScale =  TransformedWidth / OriginalRangeX;
                TransformedHeigth = MathF.Ceiling(this.OriginalRangeY * ProportationToScale) ;

            }
            else
            {
                if (MathF.Floor(Layout.MaxPictureSizeY / Layout.HexPerLine) > this.HexagonSize)
                {
                    this.HexagonSize = MathF.Floor(Layout.MaxPictureSizeY / Layout.HexPerLine);
                }
                TransformedHeigth   = Layout.MaxPictureSizeY;
                ProportationToScale =  TransformedHeigth / (OriginalMaxY - OriginalMinY);
                TransformedWidth = MathF.Ceiling( OriginalRangeX * ProportationToScale);

            }
            this.TransformedRangeX = this.OriginalRangeX * ProportationToScale;
            this.TransformedRangeY = this.OriginalRangeY * ProportationToScale;
            this.TransformedMaxX = this.OriginalMaxX * ProportationToScale;
            this.TransformedMaxY = this.OriginalMaxY * ProportationToScale;
            this.TransformedMinX = this.OriginalMinX * ProportationToScale;
            this.TransformedMinY = this.OriginalMinY * ProportationToScale;

            this.Layout = Layout;
            MaxProportion = OriginalRangeX > OriginalRangeY ? OriginalRangeX : OriginalRangeY;
        }
        private void SetTransformed()
        {
            
        }
        public ImageDefinition(Column ColumnX, Column ColumnY, Column ColumnDistance, Column ColumnXYDistinct, Point Scale = null)
        {
            if (Scale == null)
                Scale = new Point(10000, 10000);
             this.Scale = Scale;
            this.OriginalMinX = float.Parse(ColumnX.MinValue.ToString(), CultureInfo.InvariantCulture);
            
            this.OriginalMaxX = float.Parse( ColumnX.MaxValue .ToString(),CultureInfo.InvariantCulture); ;
            this.OriginalMinY = float.Parse(ColumnY.MinValue.ToString(), CultureInfo.InvariantCulture); ;
            this.OriginalMaxY = float.Parse(ColumnY.MaxValue.ToString(), CultureInfo.InvariantCulture); ;
            
            this.OriginalRangeX = this.OriginalMaxX - this.OriginalMinX;
            this.OriginalRangeY = this.OriginalMaxY - this.OriginalMinY;
            
            MaxProportion = OriginalRangeX > OriginalRangeY ? OriginalRangeX : OriginalRangeY;

            if (this.OriginalRangeX > this.OriginalRangeY)
            {


                this.TransformedWidth = Scale.X ;
                this.ProportationToScale = MathF.Floor(TransformedWidth / OriginalRangeX);
                TransformedHeigth = MathF.Ceiling(this.OriginalRangeY * ProportationToScale);

            }
            else
            {
                

                
                TransformedHeigth = Scale.Y;
                ProportationToScale = MathF.Floor(TransformedHeigth / OriginalRangeY);
                TransformedWidth = MathF.Ceiling(OriginalRangeX * ProportationToScale);

            }
            this.TransformedRangeX = this.OriginalRangeX * ProportationToScale;
            this.TransformedRangeY = this.OriginalRangeY * ProportationToScale;
            this.TransformedMaxX = this.OriginalMaxX * ProportationToScale;
            this.TransformedMaxY = this.OriginalMaxY * ProportationToScale;
            this.TransformedMinX = this.OriginalMinX * ProportationToScale;
            this.TransformedMinY = this.OriginalMinY * ProportationToScale;

            this.HexagonSize = (int)(TransformedRangeX * TransformedRangeY) / ColumnXYDistinct.NumberOfRows; ;
            this.Layout = new Layout
            {
                FillPolygon = false,
                Flat = true,
                HexPerLine = (int)(this.TransformedWidth * this.TransformedHeigth / ColumnX.NumberOfRows),
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

