using System;
using System.Drawing;
namespace IPolygonal.Model
{
    public interface IPolygon : IEquatable<IPolygon>
    {
        public string ToString() ;
        public float Length();
        public float Value { get; set; }
        public float PorcentualXaxisPosition
        {

            get; set;
        }
        public float PorcentualYaxisPosition
        {
            get; set;
        }
        public string Color
        {
            get; set;
        }
        public float Opacity { get; set; }
        public string BorderColor { get; set; }
        public string BorderType { get; set; }

    }
}
