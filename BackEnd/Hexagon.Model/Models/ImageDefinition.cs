using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hexagon.Model.Models
{
    public struct ImageDefinition
    {
        public ImageDefinition(List<Model.Point> PointsToTransform, Layout Layout)
        {
            this.OriginalMinX = PointsToTransform.Min(x => x.X);
            this.OriginalMaxX = PointsToTransform.Max(x => x.X);
            this.OriginalMinY = PointsToTransform.Min(x => x.Y);
            this.OriginalMaxY = PointsToTransform.Max(x => x.Y);
            this.OriginalRangeX = this.OriginalMaxX - this.OriginalMinX;
            this.OriginalRangeY = this.OriginalMaxY - this.OriginalMinY ;
            this.HexagonSize = 2f;
            if (this.OriginalRangeX > this.OriginalRangeY)
            {
                if (MathF.Floor(Layout.MaxPictureSizeX / Layout.HexPerLine) > this.HexagonSize)
                {
                    this.HexagonSize = MathF.Floor(Layout.MaxPictureSizeX / Layout.HexPerLine);
                }

                this.TransformedWidth = Layout.HexPerLine * this.HexagonSize;
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
            TransformedWidth = TransformedWidth + HexagonSize * 2;
            TransformedHeigth  = TransformedHeigth + MathF.Ceiling( HexagonSize * MathF.Sqrt(3));
            MaxProportion = OriginalRangeX > OriginalRangeY ? OriginalRangeX : OriginalRangeY;
        }
        public float OriginalMinX { get;  }
        public float OriginalMinY { get; }
        public float OriginalMaxX { get;  }
        public float OriginalMaxY { get; }

        public float OriginalRangeX { get; }
        public float OriginalRangeY { get  ;  }
        public float HexagonSize { get; }
        public float ProportationToScale { get; }
        public float TransformedWidth { get; }
        public float TransformedHeigth { get; }

        public float MaxProportion { get; }
        

    }
}
