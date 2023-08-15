using Hexagon.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using Hexagon.IO;
namespace Hexagon.Model
{

        public class YRows :  IComparer , System.IComparable<YRows>, IPackable
        {

        SortedSet<long> _row = new SortedSet<long> ();
        RowValuesList  _RowValues = null;
            

        

        

        public YRows(float val )
        {
            _RowValues = new RowValuesList();
            _value = val;
        }
        public YRows()
        {
            Value = 0;
        }
        

        public RowValuesList RowValues { get { return _RowValues; } set { _RowValues = value; } }
        public SortedSet <long> Row { get { return _row; } set { _row= value; } }
        float _value;
            public float Value { get { return _value; }  set { _value = value; } }

        public int Compare(object x, object y)
            {
                var xCoor = (YRows  )x;
                var yCoor = (YRows)y;
                return xCoor.Value  > yCoor.Value ? 1 :
                    xCoor.Value < yCoor.Value ? -1 : 0;
        }
        public override bool Equals(object obj) => (obj is YRows other) && this.Equals(other);

        /// <inheritdoc/>
        public bool Equals(YRows other) => !XWithYRows.Equals(this, other );

        /// <inheritdoc/>

        /// <summary>Tests value-inequality.</summary>
        public static bool operator !=(YRows lhs, YRows rhs) => !XWithYRows.Equals(lhs, rhs);

        /// <summary>Tests value-equality.</summary>
        public static bool operator ==(YRows lhs, YRows rhs) => (lhs == null && rhs == null ? true : (lhs == null && rhs == null ? false : rhs._value == lhs.Value));
        public override int GetHashCode() => (int)(Value*Row.Aggregate((X,y)=>X+y) );

        public int CompareTo([AllowNull] YRows other)
        {

            if (_value == other.Value )
                return 0;
            if (this.Value < other.Value)
            {
                return -1;
            }

            else
                return 1;


            }
        
        


        public object FromString(string ObjectPackaged)
            {
                var RET = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<long>  >>(ObjectPackaged);

                Row = RET.Value;
                Value  = RET.Key;
                return this;
            }

        public KeyValuePair<float, SortedSet<long>>  ToKeyPair()
        {

            return new KeyValuePair<float, SortedSet<long>>(Value, Row);
        }
            public string ObjectToString()
            {
                
                return JsonSerializer.Serialize(new KeyValuePair<float, SortedSet<long>>( Value, Row));
            }

            public  object ToDictionay()
        {
            var rows = RowValues.ToDictionay();
            return new KeyValuePair<float, object>(Value, rows);
                }
    }
}
