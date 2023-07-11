using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hexagon.Model.Models
{
    public interface IPackable    
    {
        public string ObjectToString();
        public object FromString(string ObjectPackaged);
    }
    public class ToPack  

    {
        public string Name { get; }
        public TypeInfo TypeInfoType { get; }
        public int  Order{ get; }

        public bool IsArray { get; }
        public TypeInfo TypeInfoArray { get; }
        public ToPack (string _Name, bool _IsArray , TypeInfo _TypeInfoType, TypeInfo TypeInfoArray,int _Order)
        {
            Name = _Name;
            IsArray = _IsArray;
            TypeInfoType = _TypeInfoType;
            TypeInfoArray = _TypeInfoType;
        }
        

    }
}
