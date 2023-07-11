using Hexagon.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace Hexagon.Model
{

        public class XWithYRows :   System.IComparable<XWithYRows >, IPackable
        {

            SortedSet <YRows> _row = new SortedSet<YRows>();
        float _value;
        public XWithYRows(float val, SortedSet<YRows> row)
        {
            _value = val;
            _row = row;
        }
        public XWithYRows(float val )
        {
            _value = val;

        }

        public XWithYRows( )
        {
            _value = float.MinValue;

        }
        public void AddYSetOFRow(float val, List<long > row)
        {
            if(_row.Where (x=>x.Value==val).Count()==0)
            {
                var newY = new YRows(val, row);
                _row.Add(newY);
            }
            else
            {
                var yRows = _row.First(x => x.Value == val);

                var newSorted = new SortedSet<long>(row);
                
                 yRows.Row.UnionWith(newSorted);
                _row.RemoveWhere(X=>X.Value==val);
                _row.Add(yRows);
            }
        }
        public void AddYRows(YRows YRows)
        {
            this.Row.Add(YRows);
        }

        public void AddYSetOFRow(float val,  long row)
        {

            AddYSetOFRow(val, new long[] { row }.ToList());
        }

        public SortedSet<YRows> Row { get { return _row; } set { _row= value; } }

        public float Value { get { return _value; } set { _value = value; } }

        public int Compare(object x, object y)
            {
                var xCoor = (XWithYRows   )x;
                var yCoor = (XWithYRows )y;
                return xCoor.Value  > yCoor.Value ? 1 :
                    xCoor.Value < yCoor.Value ? -1 : 0;
        }
        

        /// <inheritdoc/>
        public bool Equals(XWithYRows  other) => XWithYRows.Equals(this, other );

        /// <inheritdoc/>

        /// <summary>Tests value-inequality.</summary>
        public static bool operator !=(XWithYRows  lhs, XWithYRows  rhs) => !XWithYRows.Equals(lhs,rhs);

        /// <summary>Tests value-equality.</summary>
        public static bool operator ==(XWithYRows lhs, XWithYRows rhs) => (lhs == null && rhs ==null ? true : (lhs == null && rhs == null ? false : rhs._value==lhs.Value ));

        public override int GetHashCode() => (int)(_value );

        public int CompareTo([AllowNull] XWithYRows  other)
        {

            if (_value == other.Value )
                return 0;
            if (_value < other.Value)
            {
                return -1;
            }

            else
                return 1;


            }
        
        


        public object FromString(string ObjectPackaged)
            {
                var RET = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<YRows>  >>(ObjectPackaged);

                Row = RET.Value;
                Value  = RET.Key;
                return this;
            }
        
            public string ObjectToString()
            {
                return JsonSerializer.Serialize(new KeyValuePair<float, SortedSet<YRows>>( _value, Row));
            }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
