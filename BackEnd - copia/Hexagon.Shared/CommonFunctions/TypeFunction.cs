using System;
using System.Collections.Generic;
using System.Text;


namespace Hexagon.Shared.CommonFunctions
{
    public class TypeFunction
    {
        public static object GetTyped(string Data, object[] OtherUnknowNowInformation=null)
        {


            object DataConverteded = new object();
            bool Converted = false;
            DateTime DataAsDateTime = new DateTime();
            Converted = DateTime.TryParse(Data, out DataAsDateTime);
            DataConverteded = DataAsDateTime;
            if (!Converted)
            {
                decimal DataAsGenericNumber; ;
                Converted = Decimal.TryParse(Data, out DataAsGenericNumber);
                DataConverteded = DataAsGenericNumber;
            }
            if (!Converted)
            {
                bool DataAsBool ;
                Converted = Boolean.TryParse(Data, out DataAsBool);
                DataConverteded = DataAsBool;
            }
            if (!Converted)
            {
                DataConverteded = Data;
            }


            return DataConverteded;
        }

    }
}
