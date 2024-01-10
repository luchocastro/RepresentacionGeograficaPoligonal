using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexagon.IO;
namespace Hexagon.Model
{

    public class Hex : IEquatable<Hex>, IComparable<Hex>, IPackable  
    {


        public Hex(float q = 0, float r = 0, float s = 0)
        {
            if (Math.Round(q + r + s) != 0) throw new Exception("q + r + s must be 0");
            
            R = r;
            S = s;
            Q = q;
            this.Value = 0;
            this.Color = null;
            this.PorcentualXaxisPosition = 0;

            this.PorcentualYaxisPosition = 0;

            this.BorderColor = null;
            
            this.BorderType = null;
            this.Opacity = 100;
            this.RGBColor = new int[] { 254, 254, 254 };
            this.Values = null;
            this.Hexagonos = null;
            this.HexagonDetailsID = "";
        }

        
        public float Q { get; set; }
        public float R { get; set; }
        public float S { get; set; }
        public List<EventPoint> Values { get; set; }
        public string ListValues
        { get {
                string ret = "Hexagon:{";
                ret += "Q:" + Q.ToString() + ",";
                ret += "R:" + R.ToString() + ",";
                ret += "S:" + S.ToString() + ",";
                ret += "Value:" + Value.ToString();
                if (Values != null)
                {
                    ret += ",Values:[";
                    foreach (var item in Values)
                    {
                        ret += item.ListValues + ",";
                    }
                    ret += "]";
                }
                ret += "}";
                return ret; } }

        public List<Hex> Hexagonos { get; }
        
        public float Length()
        {
            return (Math.Abs(this.Q) + Math.Abs(this.R) + Math.Abs(this.S)) / 2;
        }
        #region Value Equality with IEquatable<T>
        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj is Hex other) && this.Equals(other);

        /// <inheritdoc/>
        public bool Equals(Hex other) => (Q == other.Q && R == other.R && S == other.S);

        /// <inheritdoc/>

        /// <summary>Tests value-inequality.</summary>
        public static bool operator !=(Hex lhs, Hex rhs) => !lhs.Equals(rhs);

        /// <summary>Tests value-equality.</summary>
        public static bool operator ==(Hex lhs, Hex rhs) => lhs.Equals(rhs);

        public override int GetHashCode() => (int)(Q * R * S);

        public int CompareTo([AllowNull] Hex other)
        {
            if (this == other)
                return 0;
            if (this.Q<other.Q)
            {
                if (this.R < other.R)
                {
                    if (this.S < other.S)
                        return -1;
                    else
                        return 1;
                }
                else
                    return 1;
            }
            else
                return 1;


        }
        public override string ToString()
        {
            return ObjectToString();
        }


        public string ObjectToString()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
                
                var values = JsonSerializer.Serialize(new float[] { this.Q, this.R, this.S, this.Value });
            ret.Add("H", values);
            var Corners = "";
            if (this.Corners!=null && this.Corners.Length > 0)
            {

                ret.Add("C", JsonSerializer.Serialize(this.Corners));
            }
            if (this.Rows != null && this.Rows.Count  > 0)
            {

                ret.Add("R", JsonSerializer.Serialize(this.Rows.Distinct() ));
            }
            return JsonSerializer.Serialize(ret);

        }
            public object FromString(string ToUnPackaged )
        {
            var retall = JsonSerializer.Deserialize<Dictionary<string, string>>(ToUnPackaged);
            var ret  = JsonSerializer.Deserialize <float[]>(retall ["H"]);
            

            this.Q = ret[0];
            this.R = ret[1];
            this.S   = ret[2];
            this.Value = ret[3];
            if (retall.ContainsKey("C"))
            {
                this.Corners = JsonSerializer.Deserialize<System.Drawing.PointF[]>(retall["C"]) ;
            }
            if (retall.ContainsKey("R"))
            {
                this.Rows = JsonSerializer.Deserialize<long[]>(retall["R"]).ToList() ;
            }
            return this;
        }

        public string Properties()
        {
            throw new NotImplementedException();
        }


        #endregion
        [JsonIgnore]
        public List<long> Rows{ get; set; } = new List<long>();
        public float Value { get; set; }
        public float PorcentualXaxisPosition {
            get; set; }
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
        public System.Drawing.PointF[] Corners { get; set; }  
        [JsonIgnore]
        public string HexagonDetailsID { get; set; }

    }
}
