using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexagon.Model.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class ModelSaveAtributes : Attribute
    {
        
        public bool IsBlob { get; set; }
        public string PlaceForBlob{ get; set; }
    public static bool ProperIsBlobs  (PropertyInfo  Prop)
        {
            var a = Prop.GetCustomAttributes<ModelSaveAtributes>();
            if (a.Count() == 0)
                return false;
            return a.Select(a => a.IsBlob).FirstOrDefault();
        }
        public static string HasPlaceForBlob(PropertyInfo Prop)
        {
            var a = Prop.GetCustomAttributes<ModelSaveAtributes>();
            if (a.Count() == 0)
                return "";
            return a.Select(a => a.PlaceForBlob).FirstOrDefault();
        }
    }
}
