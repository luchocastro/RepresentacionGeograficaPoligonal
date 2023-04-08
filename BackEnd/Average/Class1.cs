using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
//https://www.codeproject.com/articles/27340/a-user-friendly-c-descriptive-statistic-class
namespace Average
{
    public class Statistics
    {
        public  float Average2(Dictionary<string, object[]> Values)
        {
            var ToCalc = Values.FirstOrDefault().Value;

            var res = ToCalc.Average(x => (float)x);
            return res;
        }
        public static float Average(Dictionary<string, object[]> Values)
        {
            var ToCalc = Values.FirstOrDefault().Value;
             
            var res = ToCalc.Average  (x => (float)x);
            return res;
        }
        public static float Median(Dictionary<string, object[]> Values)
        {
            var ToCalc = Values.FirstOrDefault().Value.Select(x=> Convert.ToDouble( x.ToString(), CultureInfo.InvariantCulture));
            int numberCount = ToCalc.Count();
            double  halfIndex = ToCalc.Count() / 2;
            double halfIndexMinus = halfIndex - 1;
            var sortedNumbers = ToCalc.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt((int)Math.Floor(halfIndex)) +
                    sortedNumbers.ElementAt((int)Math.Ceiling(halfIndex))) / 2d);
            }
            else
            {
                median = sortedNumbers.ElementAt((int)Math.Floor( halfIndex));
            }

            return (float)median;
        }
        public static float Mode(Dictionary<string, object[]> Values)
        {
            var ToCalc = Values.FirstOrDefault().Value.Select(x => (float)x);

            var mode = ToCalc.GroupBy(n => n).
    OrderByDescending(g => g.Count()).
    Select(g => g.Key).FirstOrDefault();

            return mode;
        }
        public static float Return0(Dictionary<string, object[]> Values)
        {
            return 0;
        }
    }
}
