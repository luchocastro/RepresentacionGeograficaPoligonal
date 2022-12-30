using System;
using System.Collections.Generic;
using System.Linq;

namespace Average
{
    public class Average
    {
        public static float Get(List<float> Values)
        {
            float ret = 0;
            ret = Values.Sum() / Values.Count();
            return ret;
        }
    }
}
