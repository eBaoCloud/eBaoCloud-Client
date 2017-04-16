using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub
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

        public static string ToVehicleCode(VehicleUsage vehicleUseage)
        {
            switch (vehicleUseage)
            {
                case VehicleUsage.PRIVATE:
                    return "1.10";
                case VehicleUsage.RENT:
                    return "2.10";
				case VehicleUsage.PUBLIC_RENT:
					return "3.10";
                default:
                    return "1.10";
            }
        }

    }
}
