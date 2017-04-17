using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;

namespace com.ebao.gs.ebaocloud.sea.seg.client.pub
{
    class Utils
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string FormatDate(DateTime time)
        {
            return time.ToString("dd/MM/yyyyTHH:mm:ss.fff");
        }

        public static string ToVehicleGarageType(VehicleGarageType garageType)
        {
            switch (garageType)
            {
                case VehicleGarageType.GARAGE:
                    return "Garage";
                case VehicleGarageType.DEALER:
                    return "Dealer";
                default:
                    return "Garage";
            }
        }

        public static Boolean IsArray(object o)
        {
            if (null == o) return false;
            return o.GetType().BaseType == typeof(Array);
        }

    }
}
