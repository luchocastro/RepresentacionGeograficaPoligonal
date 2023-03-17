using System;
using System.Collections.Generic;
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
            var ToCalc = Values.FirstOrDefault().Value.Select(x=>(float)x );
            int numberCount = ToCalc.Count();
            int halfIndex = ToCalc.Count() / 2;
            int halfIndexMinus = halfIndex - 1;
            var sortedNumbers = ToCalc.OrderBy(n => n);
            float median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) +
                    sortedNumbers.ElementAt(halfIndexMinus)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }
        public static float Mode(Dictionary<string, object[]> Values)
        {
            var ToCalc = Values.FirstOrDefault().Value.Select(x => (float)x);

            var mode = ToCalc.GroupBy(n => n).
    OrderByDescending(g => g.Count()).
    Select(g => g.Key).FirstOrDefault();

            return mode;
        }

    }
}
