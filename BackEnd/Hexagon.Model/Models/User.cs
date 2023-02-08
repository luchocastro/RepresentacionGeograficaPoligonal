using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class User : IModelPersistible
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserSelectedOpctions UserSelectedOpctions { get; set; }
        public string ID { get ; set ; }
    }
}
