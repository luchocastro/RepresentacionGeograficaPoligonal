using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hexagon.Model
{

    public struct Hex : IEquatable<Hex>
    {
        

        public Hex(float q =0, float r = 0, float s = 0)
            {
            if (Math.Round(q + r + s) != 0) throw new Exception("q + r + s must be 0");

            R = r;
            S = s;
            Q = q;
            this.Value = 0;
            this.Color= null;
            this.PorcentualXaxisPosition = 0;

            this.PorcentualYaxisPosition= 0;

            this.BorderColor = null;

            this.BorderType= null;
            this.Opacity = 100;
            this.RGBColor = new int[] { 254, 254, 254 };
            this.Values = null;
            this.Hexagonos = null;
            
        }
        

        public float Q { get; set; }
        public float R { get; set; }
        public float S { get; set; }
        public List <EventPoint> Values { get; set; }
        public string ListValues
        { get {
                string ret = "Hexagon:{";
                ret += "Q:" + Q.ToString() + ",";
                ret += "R:" + R.ToString() + ",";
                ret += "S:" + S.ToString() + ",";
                ret += "Value:" +Value.ToString() + ",Values:[";
                foreach (var item in Values)
                {
                    ret += item.ListValues + ",";
                }
                ret += "]}";
                return ret; } }

        public List<Hex> Hexagonos { get; }
        public override string ToString() => $"({Q}, {R} , {S}  )";
        public float Length()
        {
            return (Math.Abs(this.Q) + Math.Abs(this.R) + Math.Abs(this.S)) / 2;
        }
        #region Value Equality with IEquatable<T>
        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj is Hex other) && this.Equals(other);

        /// <inheritdoc/>
        public bool Equals(Hex other) => (Q==other.Q && R==other.R && S==other.S);

        /// <inheritdoc/>

        /// <summary>Tests value-inequality.</summary>
        public static bool operator !=(Hex lhs, Hex rhs) => !lhs.Equals(rhs);

        /// <summary>Tests value-equality.</summary>
        public static bool operator ==(Hex lhs, Hex rhs) => lhs.Equals(rhs);

        public override int GetHashCode() => (int)(Q*R*S );
        #endregion

        public float Value { get; set; } 
        public float PorcentualXaxisPosition { 
        get; set;}
        public float PorcentualYaxisPosition
        {
            get; set;
        }
        public string Color
        {
            get; set;
        }

        public int[] RGBColor
        {
            get; set;
        }
        public float Opacity { get; set; }
        public string BorderColor { get; set; }
        public string BorderType { get; set; }
        
    }
}
