using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hexagon.Model.Helper
{
    public class Utils
    {
        public static string FloatFromFloat(float Num)
        {
            return Num.ToString(CultureInfo.InvariantCulture);
        }

        public static float FloatFromString(String Num)
        {
            return float.Parse( Num , CultureInfo.InvariantCulture);
        }
    }
}
