using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexagon.IO;
namespace Hexagon.Model.Models
{
    public class HexagonDetails : Base,  IPackable 
    {
        Base Base = null;
        public HexagonDetails() : base()
        {
            
        }


        public object FromString(string Dets)
        {

            List.AddRange(JsonSerializer.Deserialize<List<string>>(Dets).Select(x => (HexagonDetail)(new HexagonDetail().FromString(x))));
            return this;
        }

        public string ObjectToString()
        {

            return JsonSerializer.Serialize(List.Select(x=>x.ObjectToString() ));
        }
        public List<HexagonDetail> List { get; set; } = new List<HexagonDetail>();
            public HexagonDetails OrderList()
        {
            var ListToOrder =new List<HexagonDetail>();
            long Num = 1;
            foreach (var item in this.List)
            {
                item.NumOrder = Num;
                Num++;
                ListToOrder.Add(item);
            }
            this.List = ListToOrder;
            return this;
        }
        public override string ToString()
        {
            return ObjectToString();
        }
    }
    public struct HexagonPosition : IPackable
    {
       
        public float Q { get; set; }
        public float R { get; set; }
        public float S { get; set; }
        
        
        public string ObjectToString()
        {

            return JsonSerializer.Serialize(QRS);

        }
        public HexagonPosition FromFloat(float[] Hex )
        {
            
            this.Q = Hex[0];
            this.R = Hex[1];
            this.S = Hex[2]; ;

            return this;
        }

        public object FromString(string ToUnPackaged)
        {
            var Hex = JsonSerializer.Deserialize< float[] > (ToUnPackaged);
            ;
            this.Q = Hex [0]; 
            this.R=  Hex [1];
            this.S = Hex [2]; ;

            return this;
        }
        public float[] QRS { get { return new float[] { Q, R, S }; } }
        public override string ToString()
        {
            return ObjectToString();
        }
    }
    public class HexagonDetail  : IPackable 
    {
        public long NumOrder { get; set; }
        public HexagonDetail()
        {

        }
        [ModelSaveAtributes(InPackage = true, PropertyOrder = 1)]
        public List<long> IndexLines
        {
            get; set;
        } = new List<long>();

        [ModelSaveAtributes(InPackage = true, PropertyOrder = 0)]
        public HexagonPosition HexagonPositionForValues { get; set; } = new HexagonPosition();

        public string ObjectToString()
        {
            return JsonSerializer.Serialize(new KeyValuePair<float[],List<long>> (HexagonPositionForValues.QRS, IndexLines ));
        }

        public object FromString(string ObjectPackaged)
        {
            var val = JsonSerializer.Deserialize<KeyValuePair<float[], List<long>>>(ObjectPackaged); 
            IndexLines = val.Value ;
            HexagonPositionForValues = new HexagonPosition().FromFloat(val.Key);
            return this;
        }
    }
}
