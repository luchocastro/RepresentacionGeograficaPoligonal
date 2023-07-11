using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexagon.Model
{
    public class Pack<T> where T:IForPack
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
    public interface IForPack

    {

        public string ToString();
        public string[] ValuesPackaged(object  ForPackaged);
        public IForPack FromString(string ToUnPackaged, string Properties = "");
        public string Properties();
        public String GetValue (IForPack Father, IForPack Child, string Data);
        public object Pack { get; set; }
        public Type TypeOfPack { get; set; }
    }
}

