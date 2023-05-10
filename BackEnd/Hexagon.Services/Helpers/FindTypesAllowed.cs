using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Hexagon.Services.Helpers
{
    public class FindTypesAllowed
    {

        /*
                                            if (Line.Fieds[i] == null || Line.Fieds[i].ToString() == "")
                                    {
                                        var vacio = TiposPermitidosColumas[i][EnumAlowedDataType.NullOrEmpty];
        vacio++;
                                        TiposPermitidosColumas[i][EnumAlowedDataType.NullOrEmpty] = vacio;
                                    }
                                    else if (FindTypesAllowed.IsNumber(Line.Fieds[i], FileData.DecimalSeparator))
                                    {
                                        var q = TiposPermitidosColumas[i][EnumAlowedDataType.GenericNumber];
    q++;
                                        TiposPermitidosColumas[i][EnumAlowedDataType.GenericNumber] = q;
                                    }
                                    else
{
    var EncontroFecha = false;
    if (TieneFecha)
    {
        if (FindTypesAllowed.IsDate(Line.Fieds[i], FileData.DatetimeFormart))
        {
            EncontroFecha = true;
            var Esfecha = TiposPermitidosColumas[i][EnumAlowedDataType.DataTime];
            Esfecha++;
            TiposPermitidosColumas[i][EnumAlowedDataType.DataTime] = Esfecha;
        }
    }

    if (TieneParDenumeros && !EncontroFecha)
    {
        if (FindTypesAllowed.IsManyNumber(Line.Fieds[i], FileData.DecimalSeparator, NumberConcatenator.ToString()))
        {
            var q = TiposPermitidosColumas[i][EnumAlowedDataType.Position];
            q++;
            TiposPermitidosColumas[i][EnumAlowedDataType.Position] = q;
        }
    }
}
*/
public static int TypesAllows(string[] values, string Mask, EnumAlowedDataType DataTypeToTest)
        {
            List<string> MaskParts = new List<string>();
            string Actual = Mask.Substring(0, 1);
            for (int i = 1; i < Mask.Length; i++)
            {
                if (Actual.Substring(0, 1) == Mask.Substring(i, 1))
                    Actual = Actual + Mask.Substring(i, 1);
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
            var provider = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
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
                    provider.NumberFormat.NumberGroupSeparator = nfi.NumberGroupSeparator;
                    
                    var num = decimal.Parse(value, provider);

                    ret.Add(EnumAlowedDataType.GenericNumber);
                }
                catch (Exception rc)
                {

                }
            }

            return ret;
        }
        public static bool IsDate(string Value, string DatetimeFormart, List<string> SimbolosPermitidos = null)
        {
            var provider = CultureInfo.CurrentCulture;
            if (DatetimeFormart != null && DatetimeFormart != "")
            {
                try
                {
                    var Result = new DateTime();
                    return DateTime.TryParseExact(Value, DatetimeFormart, provider, DateTimeStyles.AdjustToUniversal, out Result);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
        public static bool IsManyNumber(string Value, string DecimalSeparator,string Spliter)
        {
            var ret = false;
            try
            {
                var SimbolosPermitidos = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(a => a.NumberFormat.CurrencyGroupSeparator);
                var SplitNumbers = Value.Split(Spliter);
                foreach (var item in SplitNumbers)
                {
                    ret = true;
                    SimbolosPermitidos.Select(a => Value = Value.Replace(a, ""));
                    Value.Replace(DecimalSeparator == "," ? "." : ",", "");
                    if (!Regex.IsMatch(Value, @"/^(?!-0(\.0+)?$)-?(0|[1-9]\d*)(\" + DecimalSeparator + @".\d+)?$/"));
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return ret;
        }
        public static bool IsNumber(string Value, string DecimalSeparator )
        {
            try {
                var SimbolosPermitidos = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(a => new String[]{ a.NumberFormat.CurrencySymbol, a.NumberFormat.PercentGroupSeparator,
                a.NumberFormat.NumberGroupSeparator, a.NumberFormat.CurrencyGroupSeparator} ).SelectMany(a=>a);
                SimbolosPermitidos.Select(a => Value = Value.Replace(a, ""));
                Value.Replace(DecimalSeparator == "," ? "." : ",", "");
                return Regex.IsMatch(Value, @"^-?[0-9]*\" + DecimalSeparator+ @"?[0-9]+$");
            }
            catch (Exception)
            {
                return false;
            }
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
