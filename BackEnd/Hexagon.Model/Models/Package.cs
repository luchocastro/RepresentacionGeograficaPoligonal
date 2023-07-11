using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  System.Text.Json;
using Hexagon.Model.Models;
using System.Reflection;
namespace Hexagon.Model 
{
    public class Package
    {
     
        public List<KeyValuePair<int, PropertyInfo>> PorpertiesOrdered<T>( )
        {
            if (CustomAttributesHelper.GetAttributes<T>(typeof(ModelSaveAtributes)).Where(x => x.Value.Count() > 0).Count()>0)
                return CustomAttributesHelper.GetAttributes<T>(typeof(ModelSaveAtributes)).Where(x => x.Value.Count()>0 ).Select(x => new KeyValuePair<int, PropertyInfo>(((ModelSaveAtributes)x.Value.First(y => y.GetType() == typeof(ModelSaveAtributes))).PropertyOrder, (PropertyInfo)x.Key)).ToList();
            
            //var props = members.Select(c => new KeyValuePair<int, PropertyInfo>(((ModelSaveAtributes) c.GetCustomAttributes().Where(x=> x.GetType() == typeof(ModelSaveAtributes)).First()).PropertyOrder,(PropertyInfo) c));

            //var atr = props.ToList() ;
            return null ;
        }
        private Type Type;
        private Attribute _Attribute;
        public Package ()
        {

        }
        public Package (Type T)
        {
            Type = T;
        }   
        public Type Attribute { get; set; } = typeof( ModelSaveAtributes);
        public List<PropertyInfo> Properties ()
        {
            if ( Type.GetMembers().Where(x => x.MemberType == MemberTypes.Property && x.GetCustomAttributes().Where(z => z.GetType() == Attribute).Count() > 0).Count()>0)
                return Type.GetMembers().Where(x => x.MemberType== MemberTypes.Property  && x.GetCustomAttributes().Where(z => z.GetType() == Attribute).Count() > 0 ).Select(z => (PropertyInfo) z).ToList();
            return null;
        }
        public List<PropertyInfo> AllProperties ()
        {
            return Type.GetMembers().Where(x => x.MemberType == MemberTypes.Property   ).Select(z => (PropertyInfo)z).ToList();

        }
        public string PropertiesList ()
        {
            if(Properties ()!=null)
            return String.Join( ",",Properties ().Select(x => x.Name ).OrderBy(x=>x).ToArray());
            return "";
        }
        public string ToString (Object  Entity )
        {

            var Props = Properties   ();
            var Ret = new List<string>();
            if (Props != null)
            {
            
                foreach (var item in Props.OrderBy(x => x.Name))
                {
                    Ret.Add(item.GetValue(Entity).ToString());
                }
                Ret.Add(Entity.GetType().FullName);
                return JsonSerializer.Serialize(Ret.Select(x =>  x  ).ToArray());
                

            }
            else
            {

                Ret.Add(Entity.ToString());
                Ret.Add(Entity.GetType().FullName);
                 ;
            }
            return JsonSerializer.Serialize(Ret.Select(x => x).ToList());

        }

        public Dictionary<string, string> ListPropertiesValue (string Data, string PropertyList="")
        {
             if(PropertyList=="")
                PropertyList = this.PropertiesList ();
            var value = new Dictionary<string, string>();
            if (PropertyList == "")
            { return value; }
            var Parsed = JsonSerializer.Deserialize<string[]>(Data); 
            int pos = 0;
            var PropertyListSplited= PropertyList.Split(",");
            foreach (var item in PropertyListSplited)
            {
                if (pos<Parsed.Length  )
                {
                    value.Add(item, Parsed[pos]);
                }
                else break;
                pos++;
            }


                    if (!value.ContainsKey("TypeParsed"))
                        value.Add("TypeParsed", Parsed[Parsed.Count()-1]);
               
            
                //foreach (var item in PropertyList.Split(","))
                //{

                //    var abierto = 0;
                //    var fin = 0;
                //    for (int i = 0; i < Data.Length; i++)
                //    {
                //        fin = i;
                //        if (Data.Substring(i, 1) == "{")
                //            abierto++;
                //        if (Data.Substring(i, 1) == "}")
                //            abierto--;
                //        if (abierto <= 0)
                //            break;

                //    }

                //    if (abierto == 0 && fin > 1)
                //    {
                //        var data = Data.Substring(1, fin - 1);
                //        if (data.Substring(0, 1) == "\"")
                //            data =data.Substring(1, data.Length - 2);
                //        value.Add(item, data);
                //    }
                //    fin = Data.IndexOf("{", fin);

                //    if (fin >= 0 & Data.Length > fin)
                //    {

                //        Data = Data.Substring(fin);
                //    }
                //    else break;
                //}


                return value;
        }

        public object FromConstructor (string Data, string PropertyList = "")
        {
            Type  = GetTypeFromList(Data);
                  
            var Values = ListPropertiesValue (Data, PropertyList);
            if (Values.Count() > 0)
            {
                var ret =  Activator.CreateInstance(Type, Values.Select(x=>x.Value)); ;



                foreach (var item in Values)
                {


                    var prop = Type.GetProperties().Select(x => x).FirstOrDefault(x => x.Name.ToLower() == item.Key.ToLower().Trim());

                    var Value = Convert.ChangeType((object)item.Value, prop.PropertyType);

                    prop.SetValue(ret, Value);
                }
                return ret;

            }
            return default;
        }
        public static Type GetTypeFromList(string Data, string PropertyList = "")
        {
            try
            {
                if (Data.Split(",").Count() == 1)
                    return null;
                var des = JsonSerializer.Deserialize<string[]>(Data).ToList();
                int pos = des.Count() - 1;
                if (PropertyList != "")
                {
                    var HasType = PropertyList.Split(",").ToList();
                    pos = HasType.IndexOf("TypeParsed");
                }
                var NameType = des.ElementAtOrDefault(pos);

                var tipo = des.ElementAtOrDefault(pos);
                return Assembly.GetCallingAssembly().GetType(tipo);
            }
            catch { return null; }

        }
        public object FromValues(string Data, string PropertyList = "", Type Att = null)
        {

            var RootType = GetTypeFromList(Data);
            Type = RootType;
            if (RootType != null)
            {
                 var ret = Activator.CreateInstance ( RootType ); ;
                if (Att == null)
                    Att = typeof(ModelSaveAtributes);
                var Package = new Package (RootType);
                Package.Attribute = Att;
                var Values = Package.ListPropertiesValue(Data, PropertyList);
                if (Values.Count() > 0)
                {
                    ; ;



                    foreach (var item in Values)
                    {

                        if (item.Key == "TypeParsed")
                            continue;
                        var prop = RootType.GetProperties().Select(x => x).FirstOrDefault(x => x.Name.ToLower() == item.Key.ToLower().Trim());

                        var BranchType = GetTypeFromList(item.Value);
                        if (BranchType != null)
                        {
                            var Child = new Package (BranchType).FromValues(item.Value, "", Att);
                            prop.SetValue(ret, Child);
                        }
                        else
                        {

                            prop.SetValue(ret, Convert.ChangeType((object)item.Value, prop.PropertyType));
                        }
                    }

                }
                return ret;
            }
            return default;
            }
        
            
//        public virtual List<object> UnPackagedValue(string Dato )
//        {

//            return JsonSerializer.Deserialize<List <object>>(Dato);
//        }
//        public virtual Dictionary<object, string> DictionaryObject(string Dato)
//        {
//            return JsonSerializer.Deserialize<Dictionary<object, string>> (Dato);
//        }
//        public virtual string DoPackage(KeyValuePair<string, string> Dato)
//        {
//            return JsonSerializer.Serialize<KeyValuePair<string, string>>(Dato);
//        }
//        public T Get (List<KeyValuePair<string, object>> PackagedObject)
//        {
//            var ret =(T)Activator.CreateInstance(Type); ;


//            foreach (var item in this.PorpertiesOrdered())
//            {


//                if (PackagedObject.Count() <= item.Key)
//                {
//                    var Value = PackagedObject[item.Key];
//                    item.Value.SetValue(ret, Value);
//                }
                
//            }
//            return ret;
//        }
//        public T Get(List<object> PackagedObject)
//        {
//            var ret = (T)Activator.CreateInstance(typeof(T)); ;
//            var values = new List<KeyValuePair<int, object>>();


//            foreach (var item in this.PorpertiesOrdered())
//            {

//                ;
//                if (PackagedObject.Count() <= item.Key)
//                {
//                    var Value = PackagedObject[item.Key];
//                    if (Value != null)
//                        item.Value.SetValue(ret, Value);
//                }

//            }


//            return ret;
//        } 
//        public List<KeyValuePair<string, object>> Set (T Entity)
//        {
//            var ret = new List<KeyValuePair<string, object>>();

//            foreach (var item in this.PorpertiesOrdered())
//            {
//                var toSet = item.Value.GetValue(Entity);
//                    ret.Add(new KeyValuePair<string, object>(item.Value.Name, toSet)) ;
//                }
            
//            return ret;
//        }
//        public string SetValue(T Entity)
//        {

//            long index = 0;
//            var ret = new List<String>();
//            foreach (var item in this.PorpertiesOrdered())
//            {
                
//;               if(item.Value.GetCustomAttributes().Where(y => y.GetType() == typeof(ModelSaveAtributes)).Select(xu => ((ModelSaveAtributes)xu)).FirstOrDefault().InPackage)
//                { 
//                    var toSet = item.Value.GetValue(Entity);
//                    ret.Add( toSet.ToString());
//                }
//                else
//                {
//                    index=  long.Parse( item.Value.GetValue(Entity).ToString());

//                }

//            }
            
//            var Retorno = new Dictionary<string, string>();
            
//            return JsonSerializer.Serialize< Dictionary<string, string>>(Retorno) ;
//        }
    }
}
