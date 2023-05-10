using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public class  Field
    {
      
        public object Value { get; set; }
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get;}
        public long Index { get; set; }
        public override string ToString()
        {
            var dictionaty = new Dictionary<long, Object>();
            dictionaty.Add(Index, Value);
            return System.Text.Json.JsonSerializer.Serialize  (dictionaty); ;
        }
        public  Field FromString(string FieldString)
        {
            var Dato = System.Text.Json.JsonSerializer.Deserialize<Dictionary<long, Object >>(FieldString);
            var value =   new Object ();
            long index = 0; 
            foreach (var item in Dato.Values)
            {
                value = item;
                break;
            }
            foreach (var item in Dato.Keys )
            {
                index= item;
                break;
            }
            return new Field() { Index = index, Value = value };
        }
    }
}
