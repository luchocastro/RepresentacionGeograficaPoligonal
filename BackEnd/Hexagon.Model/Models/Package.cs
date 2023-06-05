using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  System.Text.Json;
using Hexagon.Model.Models;
using System.Reflection;
namespace Hexagon.Model 
{
    public class Package<T> 
    {
     
        public List<KeyValuePair<int, PropertyInfo>> PorpertiesOrdered( )
        {
            if (CustomAttributesHelper.GetAttributes<T>(typeof(ModelSaveAtributes)).Where(x => x.Value.Count() > 0).Count()>0)
                return CustomAttributesHelper.GetAttributes<T>(typeof(ModelSaveAtributes)).Where(x => x.Value.Count()>0 ).Select(x => new KeyValuePair<int, PropertyInfo>(((ModelSaveAtributes)x.Value.First(y => y.GetType() == typeof(ModelSaveAtributes))).PropertyOrder, (PropertyInfo)x.Key)).ToList();
            
            //var props = members.Select(c => new KeyValuePair<int, PropertyInfo>(((ModelSaveAtributes) c.GetCustomAttributes().Where(x=> x.GetType() == typeof(ModelSaveAtributes)).First()).PropertyOrder,(PropertyInfo) c));

            //var atr = props.ToList() ;
            return null ;
        }
        public List<PropertyInfo> Properties<Att>()
        {
            if ( typeof(T).GetMembers().Where(x => x.MemberType == MemberTypes.Property && x.GetCustomAttributes().Where(z => z.GetType() == typeof(Att)).Count() > 0).Count()>0)
                return typeof(T).GetMembers().Where(x => x.MemberType== MemberTypes.Property  && x.GetCustomAttributes().Where(z => z.GetType() == typeof(Att)).Count() > 0 ).Select(z => (PropertyInfo) z).ToList();
            return null;
        }
        public List<PropertyInfo> AllProperties ()
        {
            return typeof(T).GetMembers().Where(x => x.MemberType == MemberTypes.Property   ).Select(z => (PropertyInfo)z).ToList();

        }
        public string PropertList<Att>()
        {
            if(Properties<Att>()!=null)
            return String.Join( ",",Properties<Att>().Select(x => x.Name ).OrderBy(x=>x).ToArray());
            return "ToString()";
        }
        public string ToString<Att>(T Entity )
        {

            var Props = Properties  <Att>();
            if (Props != null)
            {
                var Ret = new List<string>();
                foreach (var item in Props.OrderBy(x => x.Name))
                {
                    Ret.Add(item.GetValue(Entity).ToString());
                }

                return String.Join(",", Ret.Select(x => "{" + x + "}").ToArray());
            }
            else
            {

                return Entity.ToString();
            }

        }

        public Dictionary<string, string>  PorpertiesList(string Data)
        {
             
            var Mask = PropertList<ModelSaveAtributes>();

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

                if (fin >= 0 & Data.Length > fin)
                {

                    Data = Data.Substring(fin);
                }
                else break;
            }


            return value;
        }
        public T FromValues<Att>(Dictionary<String, String> Values)
        {
            if (Properties<Att>() != null)
            {
                var Constructor = typeof(T).GetConstructor(new Type[] { typeof(Dictionary<string, string>) });

                return (T)Constructor.Invoke(new object[] { Values });
            }
            return default;
        }

        public virtual List<object> UnPackagedValue(string Dato )
        {

            return JsonSerializer.Deserialize<List <object>>(Dato);
        }
        public virtual Dictionary<object, string> DictionaryObject(string Dato)
        {
            return JsonSerializer.Deserialize<Dictionary<object, string>> (Dato);
        }
        public virtual string DoPackage(KeyValuePair<string, string> Dato)
        {
            return JsonSerializer.Serialize<KeyValuePair<string, string>>(Dato);
        }
        public T Get (List<KeyValuePair<string, object>> PackagedObject)
        {
            var ret =(T)Activator.CreateInstance(typeof(T)); ;


            foreach (var item in this.PorpertiesOrdered())
            {


                if (PackagedObject.Count() <= item.Key)
                {
                    var Value = PackagedObject[item.Key];
                    item.Value.SetValue(ret, Value);
                }
                
            }
            return ret;
        }
        public T Get(List<object> PackagedObject)
        {
            var ret = (T)Activator.CreateInstance(typeof(T)); ;
            var values = new List<KeyValuePair<int, object>>();


            foreach (var item in this.PorpertiesOrdered())
            {

                ;
                if (PackagedObject.Count() <= item.Key)
                {
                    var Value = PackagedObject[item.Key];
                    if (Value != null)
                        item.Value.SetValue(ret, Value);
                }

            }


            return ret;
        }
        public string ToString(T Entity)
        {
            return String.Concat(SetValue( Entity).Select(x=> String.Format("{0} ,", x .ToString()).ToArray())) ;
        }
        public List<KeyValuePair<string, object>> Set (T Entity)
        {
            var ret = new List<KeyValuePair<string, object>>();

            foreach (var item in this.PorpertiesOrdered())
            {
                var toSet = item.Value.GetValue(Entity);
                    ret.Add(new KeyValuePair<string, object>(item.Value.Name, toSet)) ;
                }
            
            return ret;
        }
        public string SetValue(T Entity)
        {

            long index = 0;
            var ret = new List<String>();
            foreach (var item in this.PorpertiesOrdered())
            {
                
;               if(item.Value.GetCustomAttributes().Where(y => y.GetType() == typeof(ModelSaveAtributes)).Select(xu => ((ModelSaveAtributes)xu)).FirstOrDefault().InPackage)
                { 
                    var toSet = item.Value.GetValue(Entity);
                    ret.Add( toSet.ToString());
                }
                else
                {
                    index=  long.Parse( item.Value.GetValue(Entity).ToString());

                }

            }
            
            var Retorno = new Dictionary<string, string>();
            
            return JsonSerializer.Serialize< Dictionary<string, string>>(Retorno) ;
        }
    }
}
