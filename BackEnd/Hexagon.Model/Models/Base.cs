using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class Base : IModelPersistible
    {
        public string ID { get ; set ; }
        public string ParentID { get; set; }
    }
}
