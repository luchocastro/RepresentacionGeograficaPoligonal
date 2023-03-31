using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hexagon.Services.CalcStrategy
{
    public class DoCalc
    {
        public static float Do(object[] Params, string Path, string FullClassName, string Function)
        {
            Assembly assembly = Assembly.LoadFile(Path);
            Type objtype = assembly.GetType(FullClassName);
            var method = objtype.GetMethod(Function);
            var ret = method.Invoke(objtype, Params);
            return (float)ret;

        }
        public static List<Function> GetFunctions(string FullClassName)
        {

            var ret = new List<Function>();
            Assembly assembly = Assembly.LoadFile(FullClassName);
            Type [] objtypes = assembly.GetTypes ();
            foreach (var objtype in objtypes)
            {

            var methods = objtype.GetMethods();
                foreach (var method in methods)
                {
                    if(method.IsStatic && method.IsPublic)
                    { 
                    var parameters = method.GetParameters();
                    Dictionary<string, string> Params = new Dictionary<string, string>();
                    foreach (var parameter in parameters)
                    {

                        Params.Add(parameter.Name, parameter.ParameterType.FullName  );
                    }
                    Function function = new Function( FullClassName, objtype.FullName, method.Name, Params);
                        function.Path = FullClassName;
                        ret.Add(function);
                    }
                }
            }


            return ret;

        }
    }
}
