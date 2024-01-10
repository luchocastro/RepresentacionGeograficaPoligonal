using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hexagon.IO
{
    public interface IPackable    
    {
        public string ObjectToString();
        public object FromString(string ObjectPackaged);
        
    }


}
