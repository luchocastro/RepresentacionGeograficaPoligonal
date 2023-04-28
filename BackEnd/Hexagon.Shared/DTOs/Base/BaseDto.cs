using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs.Base
{
    public class BaseDto
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Path{ get; set; }
        public bool IdTraslated { get; set; } = false;
        public virtual string Name { get; set; }
    }
}
