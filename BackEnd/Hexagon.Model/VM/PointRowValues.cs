using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexagon.IO;
namespace Hexagon.Model 
{
    public class RowValues : IPackable, IComparable, IComparer<RowValues>
    {

        ColumnValueShortedList _ColumnValueShortedList = new ColumnValueShortedList();
        public long Row { get; set; }

        
        public ColumnValueShortedList ColumnValueShortedList { get { return _ColumnValueShortedList; } set { _ColumnValueShortedList = value; } }
        KeyValuePair<long, List<short[]>> _AsKeyPair = new KeyValuePair<long, List<short[]>>();

        public object FromString(string ObjectPackaged)
        {
            _AsKeyPair = JsonSerializer.Deserialize<KeyValuePair<long, List<short[]>>>(ObjectPackaged);
            Row = _AsKeyPair.Key;
            _ColumnValueShortedList.Values  = _AsKeyPair.Value ;
            return this;
        }
        public override string ToString()
        {
            return ObjectToString();
        }

        public KeyValuePair <long, List<short[]>> AsKeyPair
        {
            get { return _AsKeyPair; }
            set
            {
                _AsKeyPair = value;
                Row = _AsKeyPair.Key;
                _ColumnValueShortedList.Values = new ColumnValueShortedList().Values;
            }
        }

        public object ToDictionay()
        {

            return AsKeyPair;

        }
        public object FromDictionay(object AsKeyPair)
        {
            this.AsKeyPair = (KeyValuePair <long, List<short[]>>)AsKeyPair;
            return this;

        }
        public string ObjectToString()
        {

            return JsonSerializer.Serialize(AsKeyPair);
        }
        public int Compare(object x, object y)
        {

            if (x == null && y == null)
                return 0;
            if (y == null)
                return 1;
            if (x == null)
                return -1;
            var xCoor = (RowValues)x;
            var yCoor = (RowValues)y;
            return xCoor.Row > yCoor.Row ? 1 :
                xCoor.Row < yCoor.Row ? -1 : 0;



        }

        public int CompareTo(object obj)
        {

            return this.Compare(this, obj);
        }

        public int Compare([AllowNull] RowValues x, [AllowNull] RowValues y)
        {



            return new RowValues().Compare(x, y);
        }
    }
    public class RowValuesList : IPackable
    {
        List<RowValues> _Values { get; set; } = new List<RowValues>();
        public IEnumerable<RowValues> Values { get { return _Values; } set { _Values =(List<RowValues>) value; } }
        public void Add(long Row , ColumnValueShortedList valueList)
        {
            _Values.Add(new RowValues() { Row = Row , ColumnValueShortedList = valueList});

        }
        

        SortedDictionary<long, List<short[]>> _SortedAsKeyPair = new SortedDictionary<long, List<short[]>>();
        public object FromKeyPairArray(List<Object> Array)
        {
            Values = Array.Select(x => (RowValues) new RowValues().FromDictionay(x))  ;
            return this;
        }


        public object FromString(string ObjectPackaged)
        {
            return FromDictionay(JsonSerializer.Deserialize<SortedDictionary<long, List<short[]>>>(ObjectPackaged));

        }
        

        public object ToDictionay()
        {
            _SortedAsKeyPair = new SortedDictionary<long, List<short[]>>();
            foreach (var item in Values)
            {
                _SortedAsKeyPair.Add(item.Row, item.ColumnValueShortedList.Values);
                
            }
            
            return _SortedAsKeyPair ;

        }
        public object FromDictionay(object AsKeyPair)
        {
            this.Values = ((SortedDictionary<long, List<short[]>>)AsKeyPair).Select(x=> (RowValues)new RowValues().FromDictionay( x)) ;
            return this;

        }
        public string ObjectToString()
        {
            return JsonSerializer.Serialize((SortedDictionary<long, List<short[]>>)ToDictionay());
        }
    
    }
}
