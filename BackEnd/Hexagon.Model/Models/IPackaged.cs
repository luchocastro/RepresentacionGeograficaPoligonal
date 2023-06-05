using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hexagon.Model.Models
{
    public interface IPackable<T> where T:IForPack<T>
    { 

    }
}
