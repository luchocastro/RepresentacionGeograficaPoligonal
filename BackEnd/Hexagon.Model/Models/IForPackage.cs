using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexagon.Model
{
    public class Pack<T> where T:IForPack<T>
    {
        public static string StringPack ( )    
        {
            var ToMasked = typeof(T) ;
            var members = ToMasked.GetMembers().Where(z => z.CustomAttributes.ToList().Count() > 0).ToList();

            var props = members.Where(x => x.MemberType == MemberTypes.Property && x.GetCustomAttributes().Where(y => y.GetType() == typeof(ModelSaveAtributes)).Count() > 0).Select(x => (PropertyInfo)x);

            string Mask = "{"+ ToMasked.Name;
            foreach (var item in props)
            {
                Mask += "'{Prop=" + item.Name + "':{'";
                var objtype = item.PropertyType.GetInterfaces().Where(x => x.Name.Contains("IForPack")).First();
                if(objtype!=null)
                {
                    Mask += "'{ " + item.Name + "':{'";
                    var method = objtype.GetMethod("DoMask");
                    var ret = method.Invoke(objtype, new object[] { item.PropertyType });
                    Mask += ret.ToString();
                }
                Mask += "}'";
            }
            Mask += "}'";
            return Mask;

        }

    }
    public interface IForPack<Y>  where Y : IForPack<Y>

    {


        public string ToString( );
        public Dictionary<string, string> ValuesPackeged(Y ForPackaged);
        public Y FromString (string ToUnPackaged);
        public string DoMask( );
        public Y GetValue ( )  ; 
}
}

