using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class Content:Base
    {
        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
