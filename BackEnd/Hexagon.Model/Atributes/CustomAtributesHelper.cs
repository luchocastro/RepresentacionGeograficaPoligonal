using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexagon.Model.Models
{
    public class CustomAttributesHelper 
    {
        public static A GetCustomAttribute<T,A>(string Name, MemberTypes MemberType) where T : class where A : Attribute
        {
            try
            {
                return (A)typeof(T).GetMember( Name, MemberType,BindingFlags.Default).Select(x=>x.GetCustomAttribute( typeof(A), false)) ;

            }
            catch (SystemException)
            {
                return default;
            }
        }
        public static List<MemberInfo> GetAttributes(Type type )
        {
            var ret = type.GetMembers().Where(z => z.CustomAttributes.ToList().Count() > 0).ToList();
            return ret;
        }

        public static List< MemberInfo>  GetAttributes <T>( )
        {
            var ret = typeof(T).GetMembers().Where(z => z.CustomAttributes.ToList().Count() > 0).ToList();  
            return ret;
        }
        public static List<KeyValuePair<MemberInfo, List<Attribute>>> GetAttributes<T>(Type typeAtt)
        {
            var ret = typeof(T).GetMembers().Select(x => new KeyValuePair<MemberInfo, List<Attribute>>(x, x.GetCustomAttributes().Select(z => z).Where(z=>z.GetType() == typeAtt).ToList()) ) .ToList();
            return ret;
        }
        public static List<KeyValuePair<MemberInfo, List<Attribute>>> GetAttribute(Type typeAtt, Type type)
        {
            var ret = type.GetMembers().Select(x => new KeyValuePair<MemberInfo, List<Attribute>>(x, x.GetCustomAttributes().Select(z => z).Where(z => z.GetType() == typeAtt).ToList())).ToList();
            return ret;
        }
    }
}
