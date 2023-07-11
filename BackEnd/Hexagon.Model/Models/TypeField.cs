using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model 
{
    public class FieldType
    {
        public FieldType() {; }


        public string FieldTypeName { get; set; }  
        public bool OwnIndexInData { get; set; }
        private List<FieldData> _FieldData = new List<FieldData>();
        public List<FieldData> FieldData { get { return _FieldData; } }
        public void Add(string TypeName, int Order, string Mask="")
        {
            _FieldData.Add(new FieldData { Mask = Mask, Order = Order, TypeName = TypeName });

        }
        
        
    }
    public class FieldData
    {

        public string Mask { get; set; } = "";
        public string TypeName { get; set; }
        public int Order { get; set; }

    }

}
