using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexagon.IO;
namespace Hexagon.Model
{
    public class ColumnValue : IPackable
    {
        [JsonIgnore]
        public string Value { get; set; } = "";
        [JsonIgnore]
        public string Column { get; set; } = "";
        string _get = "";
        [JsonIgnore] public string Get { get { return Column + ":" + Value; }
            set {
                var parsed = value.Split(":");

                Column = parsed[0];
                if (value.Length > Column.Length + 1)
                    Value = value.Substring(Column.Length + 2);
            }
        }

        public short[] ToByte { get { return Get.ToCharArray().Select(x => ((short)x)).ToArray(); }
            set { Get = new string(value.Select(x => (char)x).ToArray()); } }

        public ColumnValue FromShorts(short[] Shorts)
        {
            this.ToByte = Shorts;
            return this;
        }
        public object FromString(string ObjectPackaged)
        {
            ToByte = JsonSerializer.Deserialize<short[]>(ObjectPackaged);
            return this;
        }
        public override string ToString()
        {
            return ObjectToString();
        }

        public string ObjectToString()
        {
            return JsonSerializer.Serialize(ToByte);
        }
    }
    public class ColumnValueShortedList : IPackable
    {
        List<short[]> _Values { get; set; } = new List<short[]>();

        public List<short[]> Values { get { return _Values; } set { _Values =  value; } }
        public void Add(string Column, string value)
        {

            _Values.Add(new ColumnValue() { Column = Column, Value = value }.ToByte);

        }

        public string ObjectToString()
        {
            return JsonSerializer.Serialize(Values);
        }

        public object FromString(string ObjectPackaged)
        {
            Values = JsonSerializer.Deserialize<List<short[]>>(ObjectPackaged);
            return this;
        }
    }
    
    public class ColumnValueList:  IPackable
    {



     List<ColumnValue> _Values { get; set; } = new List<ColumnValue>();

        public IEnumerable<ColumnValue> Values { get { return _Values; }  set { _Values = (List < ColumnValue > )value; } }
        public void Add (string Column, string value )
        {

            _Values.Add (new ColumnValue() { Column = Column, Value = value });

        }
        public ColumnValueList FromshortArray(List<short[]> Array)
        {
            _Values = Array.Select(x => new ColumnValue().FromShorts(x)).ToList();
            return this;
        }

        public List<short[]> ToshortArray()
        {
            
            return Values.Select(x => x.ToByte).ToList();
        }
        public object FromString(string ObjectPackaged)
        {
            return FromshortArray(JsonSerializer.Deserialize< List<short[]>>(ObjectPackaged));
            
        }

        public string ObjectToString()
        {
            return JsonSerializer.Serialize (ToshortArray());
        }
    }
}
