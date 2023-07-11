using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Hexagon.Model.Models
{

    public class GenericPackage : IPackable
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

            if (IPackable != default)
            {


                var pacinstance = Activator.CreateInstance(Type);

                var ObjectToString = Type.GetMethods().Where(X => X.Name == "FromString").FirstOrDefault();

                var IValue = ObjectToString.Invoke(pacinstance, new object[] { Data });

                return IValue;
            }



            return Convert.ChangeType(Data, Type);
        }




        public string ObjectToString()
        {
            if (IPackable != default)
            {
                var ObjectToString = Type.GetMethods().Where(X => X.Name == "ObjectToString").FirstOrDefault();
                var IValue = ObjectToString.Invoke(Value, null).ToString();

                return IValue;
            }
            return Value.ToString();
        }


    }
}
