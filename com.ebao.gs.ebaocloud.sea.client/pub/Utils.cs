using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

    }
}
