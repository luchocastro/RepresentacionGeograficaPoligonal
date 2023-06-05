using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Hexagon.Model
{
    public class Column : Base
    {

        public Column()
        {

        }

        public Column(string Name, int OriginalPosition, EnumActionToDoWithUncasted ActionToDoWithUncasted,
        EnumAlowedDataType DataTypeSelected)
        {
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
            this.DataTypeSelected = DataTypeSelected;
        }
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get; set; } = (new EnumAlowedDataType[] { EnumAlowedDataType.Character }).ToList();
        [JsonIgnore]
        public EnumAlowedDataType DataTypeSelected { get; set; }
        string _FieldPorpertiesInValue { get; set; }
        public string FieldPorpertiesInValue { get { return _FieldPorpertiesInValue; }
            set { _FieldPorpertiesInValue = value;
                WriteFieldPorperties(); } }
        List<KeyValuePair<String, string>> _FieldPorperties = new List<KeyValuePair<String, string>>();

        public int OriginalPosition { get; set; }
        public EnumActionToDoWithUncasted ActionToDoWithUncasted { get; set; } = EnumActionToDoWithUncasted.DeleteData;
        [JsonIgnore]
        public List<Field> Fields { get; set; }
        public void ReadFieldPorperties<T>( )
        { 
            _FieldPorperties = new Package<T>().Properties <ModelSaveAtributes>().Select(x => new KeyValuePair <string, string>(x.Name,x.PropertyType.Name)).ToList();
            _FieldPorpertiesInValue = String.Join("#", _FieldPorperties.Select(x => x.Key + ";" + x.Value).ToArray() ) ; 
        }
        private void WriteFieldPorperties( )
        {
            _FieldPorperties =  new List<KeyValuePair<String, string>>();
            if (this.FieldPorpertiesInValue !=null  )
            {
                var split = this.FieldPorpertiesInValue.Split("#");
                foreach (var item in split)
                {
                    var toAdd = item.Split(";");
                    if (toAdd.Count() == 2)
                    {
                        _FieldPorperties.Add(new KeyValuePair<String, string>(toAdd[0], toAdd[1]));
                    }
                }
            }

        }

        public string PathFields { get; set; }
        public string PathSampleFields { get; set; }
        public Dictionary<EnumAlowedDataType, int> DictionaryEnumAlowedDataType { get; set; }
        public object MaxValue { get; set; } = null;
        public object MinValue { get; set; } = null;
        public long NumberOfRows{ get; set; } = 0;
    }
}
