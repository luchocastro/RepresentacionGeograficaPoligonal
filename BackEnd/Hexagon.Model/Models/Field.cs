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
    public class Field  
    {
        Package Package = new Package(typeof(Field)) ;
        List<object> _ValueList = new List<object>();
        public Field()
        {

        }
        public List<Object> ValueList
        {
            get { return _ValueList; }
            set
            {
                _ValueList = value;
                Value = _ValueList;
            }
        }

        public Field(Dictionary<string,string> Propeties)
        {
            foreach (var item in Propeties)
            {


            }

        }
        

        [ModelSaveAtributes(InPackage = true, PropertyOrder = 1)]
        public object Value { get; set; }
        public String TypeName { get; set; } = "";
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get; }

        

        

        public long Index { get; set; }






        


        public string ObjectToString()
        {

            
            return System.Text.Json.JsonSerializer.Serialize(new string[] { Index.ToString(), Value.ToString()});

        }
        
        override public string ToString()
        {


            return ObjectToString();

        }
        public Field FromString(string ObjectPackaged)
        {

            var Values = System.Text.Json.JsonSerializer.Deserialize<string[]>(ObjectPackaged);
            this.Value = Values[1];
            this.Index = long.Parse(Values[0]);
            return this;
        }
    }
}
