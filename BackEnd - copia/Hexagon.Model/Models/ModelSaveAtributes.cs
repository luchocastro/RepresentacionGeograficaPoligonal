using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexagon.Model.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ModelSaveAtributes : CustomAttributeBase
    {
        private int _PropertyOrder = 0;
        private bool _InPackage = false;
        private bool _OutPackage = false;
        public int PropertyOrder { get { return _PropertyOrder; } set { _PropertyOrder=value; } }
        public bool InPackage { get { return _InPackage; }  set { _InPackage = value; } }

        public ModelSaveAtributes()
        { 
        }



    }

}
