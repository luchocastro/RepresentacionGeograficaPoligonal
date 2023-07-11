using Hexagon.Model.FileDataManager;
using Hexagon.Model.Models;
using Hexagon.Model.Repository;
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
        RWDataColumn _RowDataColumn = null;
        public Column()
        {
            _RowDataColumn = new RWDataColumn(this);
        }

        public Column(string Name, int OriginalPosition, EnumActionToDoWithUncasted ActionToDoWithUncasted,
        EnumAlowedDataType DataTypeSelected)
        {
            new Column();
            this.ActionToDoWithUncasted = ActionToDoWithUncasted;
            this.Name = Name;
            this.OriginalPosition = OriginalPosition;
            this.DataTypeSelected = DataTypeSelected;

            _RowDataColumn = new RWDataColumn(this);
        }
        [JsonIgnore]
        public RWDataColumn RWDataColumn { get { return _RowDataColumn; } }
        [JsonIgnore]
        public List<EnumAlowedDataType> DataTypeFinded { get; set; } = (new EnumAlowedDataType[] { EnumAlowedDataType.Character }).ToList();
        [JsonIgnore]
        public EnumAlowedDataType DataTypeSelected { get; set; }
        [JsonIgnore]
        string _TypeInValueField { get; set; }
        public FieldType FieldType
        { get  ;
            set  ;  } 

        public int OriginalPosition { get; set; }
        public EnumActionToDoWithUncasted ActionToDoWithUncasted { get; set; } = EnumActionToDoWithUncasted.DeleteData;
        [JsonIgnore]
        public List<Field> Fields { get; set; }
        
        public string PathFields { get; set; }
        public string PathSampleFields { get; set; }
        public Dictionary<EnumAlowedDataType, int> DictionaryEnumAlowedDataType { get; set; }
        public object MaxValue { get; set; } = null;
        public object MinValue { get; set; } = null;
        public long NumberOfRows{ get; set; } = 0;
        public Field Field (long Pos, object Value)
        {
            var ret = new Field();
            ret.Index = Pos;
            ret.Value = new GenericPackage(Value).ObjectToString();
            return ret;
        }
        public Field ObjectFromString(string Field)
        {
            var ret = new Field();
            
            if (this.FieldType == null  || this.FieldType.FieldTypeName == null)
                return  ret.FromString(Field);
                 if (!this.FieldType.OwnIndexInData)
            {
                ret = ret.FromString(Field);
                if (  Type.GetType(this.FieldType.FieldTypeName) != null)
                {
                    var value = new GenericPackage(Type.GetType(this.FieldType.FieldTypeName)).FromString(ret.Value.ToString());
                    ret.Value = value;
                }
            }
            else
            {
                if (Type.GetType(this.FieldType.FieldTypeName) != null)
                {


                    var value = new GenericPackage(Type.GetType(this.FieldType.FieldTypeName)).FromString(Field);
                    ret.Value = value;
                    ret.Index = -1;
                }
                else
                    ret.Value = Field ;

            }

            return ret;
        }
    }
}
