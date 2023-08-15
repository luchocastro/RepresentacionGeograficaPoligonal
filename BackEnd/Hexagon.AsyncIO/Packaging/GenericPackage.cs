using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Hexagon.IO
{

    public class GenericPackage : IPackable  , IComparer<GenericPackage>, IComparable<GenericPackage >
    {
        object _Value;
        string _TextValue;

        Type Type;


        Type IPackable;

        public GenericPackage(object ToPack)
        {
            Type  = ToPack.GetType();
            

            this.Value = ToPack;
            IPackable = Type.GetInterfaces().Where(x => x.Name == typeof(IPackable).Name).FirstOrDefault();
        }
        public GenericPackage(Type ToUnPack)
        {
            Type = ToUnPack;


            IPackable = Type.GetInterfaces().Where(x => x.GetType() == typeof(IPackable)).FirstOrDefault();
        }
        public delegate void GetClass<T>(T Entity);



        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {

                _Value = value;
            }
        }


        public object FromString(string Data)
        {



            var ObjectToString = Type.GetMethods().Where(X => X.Name == "FromString").FirstOrDefault();
            if(ObjectToString!=default)
            { 
            var pacinstance = Activator.CreateInstance(Type);

                
                var IValue = ObjectToString.Invoke(pacinstance, new object[] { Data });

                return IValue;
            }
        


            return Convert.ChangeType(Data, Type);
        }




        public string ObjectToString()
        {
            
                var ObjectToString = Type.GetMethods().Where(X => X.Name == "ObjectToString").FirstOrDefault();
            if (ObjectToString != null)
            {
                var IValue = ObjectToString.Invoke(Value, null).ToString();

                return IValue;
            }
            

            return Value.ToString();
        }
        public object ObjectFromLight(string LightObject)
        {


            var ObjectToString = Type.GetMethods().Where(X => X.Name == "ObjectFromLight").FirstOrDefault();
            if (ObjectToString != default)
            {
                var pacinstance = Activator.CreateInstance(Type);


                var IValue = ObjectToString.Invoke(pacinstance, new object[] { LightObject });

                return IValue;
            }



            return Convert.ChangeType(LightObject, Type);
        }
        public object ObjectToLight()
        {

            var ObjectToString = Type.GetMethods().Where(X => X.Name == "ObjectToLight").FirstOrDefault();
            if (ObjectToString != null)
            {
                var IValue = ObjectToString.Invoke(Value, null) ;

                return IValue;
            }


            return this.ObjectToString() ;
        }
        public int Compare([AllowNull] GenericPackage x, [AllowNull] GenericPackage y)
        {
            var ObjectToString = Type.GetMethods().Where(X => X.Name == "Compare").FirstOrDefault();
            var xP = x == null ? x : x.Value;
            var yP = x == null ? y : y.Value;
            if (xP == null && yP == null)
                return 0;
            if (yP== null)
                return 1;
            if (yP== null)
                return -1;
            if (xP.GetType() ==xP.GetType () && ObjectToString != null)
            {
                var IValue = ObjectToString.Invoke(Value, new object[] { xP, yP });
                return (int)IValue;
            }
            
            else
                return string.Compare(xP.ToString() ,yP.ToString());

        }

        public int CompareTo([AllowNull] GenericPackage other)
        {
            return this.Compare(this, other );
        }
    }
}
