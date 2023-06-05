using Hexagon.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;

using System.Runtime.Serialization.Json  ;
namespace Hexagon.Model
{
    [Serializable]
    public class Field : IForPack<Field> , ISerializable
    {
        Package<Field> Package = new Package<Field>();
        public Field()
        {
        }
        protected Field(SerializationInfo si, StreamingContext context)
        {
            string value = si.GetString ("Field");
            this.FromString(value);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            string prop = "{" + Index.ToString() + "}" +
                "{" + Value.ToString() + "}";
            info.AddValue("Field", prop);
        }
        [ModelSaveAtributes(InPackage = true, PropertyOrder = 1)]
        public object Value { get; set; }
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get; }
        public void SetValue<T> (T Entity)
        {
            Value = new Package<T>().ToString(); 

        }
        [ModelSaveAtributes(InPackage = false, PropertyOrder = 0)]
        public long Index { get; set; }

        public string DoMask(  ){ return "{Index},{'Value'}"; }

        public override string ToString()
        {
            return Package.ToString<ModelSaveAtributes>(this); 
        }
        public Field FromString(string Data)
        {


            Field ret = new Field();
            var Mask = DoMask();
            Data = Data.Substring(1);
            var value = new Dictionary<string, string>();
            foreach (var item in Mask.Split(","))
            {

                var abierto = 0;
                var fin = 0;
                for (int i = 0; i < Data.Length; i++)
                {
                    fin = i;
                    if (Data.Substring(i, 1) == "{")
                        abierto++;
                    if (Data.Substring(i, 1) == "}")
                        abierto--;
                    if (abierto <= 0)
                        break;

                }

                if (abierto == 0 && fin > 1)
                    value.Add(item, Data.Substring(1, fin - 1));
                fin = Data.IndexOf("{", fin);

                if (fin>=0 & Data.Length > fin)
                {

                        Data = Data.Substring(fin);
                }
                else break;
            }

            foreach (var item in value)
            {
                if (item.Key.ToLower().Contains("index"))
                {
                    ret.Index = long.Parse(item.Value.Replace ("\"","")); 
                }
                if (item.Key.ToLower().Contains("value"))
                {
                    ret.Value= item.Value.Replace("\"", "");
                }
            }
            return ret;
        }

        public Field GetValue(Field Entity)
        {
            throw new NotImplementedException();
        }

        public Field GetValue()
        {
            throw new NotImplementedException();
        }

        public string ValuesPackeged(Dictionary<string, string> c)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ValuesPackeged(Field ForPackaged)
        {
            throw new NotImplementedException();
        }

    }
}
