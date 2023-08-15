 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.IO 
{
    public interface ISplitOptions
    {
        public SplitOptions Get();
        public static string Position { get { return "SplitOptionsPosition"; } }

    }
}
