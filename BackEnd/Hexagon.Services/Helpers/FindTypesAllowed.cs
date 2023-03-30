using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hexagon.Services.Helpers
{
    public class FindTypesAllowed
    {
        public static int TypesAllows(string [] values, string Mask, EnumAlowedDataType DataTypeToTest )
        {
            List<string> MaskParts = new List<string>();
            string Actual = Mask.Substring(0, 1); 
            for (int i = 1; i < Mask.Length; i++)
            {
                if (Actual.Substring(0, 1) == Mask.Substring(i, 1))
                    Actual  = Actual + Mask.Substring(i, 1);
                else
                {
                    MaskParts.Add(Actual);
                    Actual = Mask.Substring(i, 1);
                }
            }
            MaskParts.Add(Actual);
            return 0;

        }
            public static List<EnumAlowedDataType> GetTypesAllows(string value, DataFileConfiguration DataFileConfiguration)
        {
            var ret = new List<EnumAlowedDataType>();
            var provider = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
            if (DataFileConfiguration.DatetimeFormart != null && DataFileConfiguration.DatetimeFormart != "")
            {
                try
                {
                    var date = DateTime.ParseExact(value, DataFileConfiguration.DatetimeFormart, provider);
                    if (date == null)
                        ret.Add(EnumAlowedDataType.DataTime);
                }
                catch (Exception)
                {

                }
            }
                if (DataFileConfiguration.DecimalSeparator != null && DataFileConfiguration.DecimalSeparator != "")
                {
                    try
                    {
                        NumberFormatInfo nfi = new NumberFormatInfo();

                        nfi.NumberDecimalSeparator = DataFileConfiguration.DecimalSeparator;
                        nfi.NumberGroupSeparator = DataFileConfiguration.DecimalSeparator == "." ? "," : ".";
                        provider.NumberFormat.NumberDecimalSeparator = nfi.NumberDecimalSeparator;
                    provider.NumberFormat.NumberGroupSeparator = nfi.NumberGroupSeparator ;

                    var num = decimal.Parse(value, provider);

                        ret.Add(EnumAlowedDataType.GenericNumber);
                    }
                    catch (Exception rc)
                    {

                    }
                }
            
            return ret;
        }
        public static List<EnumAlowedDataType> TypesPrincipals(List<EnumAlowedDataType> TypesFounded, int TotalFields)
        {
            var Grupo = TypesFounded.GroupBy(x => x);
            var RetGroup = new List<EnumAlowedDataType>();
            foreach (var grupo in Grupo)
            {
                if ((double)(grupo.Count()) > TotalFields * 0.9)
                {
                    RetGroup.Add(grupo.First());
                }

            }
            return RetGroup;
        }
    }
}
