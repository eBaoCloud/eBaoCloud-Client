using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebaocloud.client.thai.seg.cmi.pub
{
    public class ApiServiceFactory
    {
        private static string host = "https://thuat.ebaocloud.com";
        public static string getCurrentHost()
        {
#if DEBUG
            host = "http://172.18.49.49:8888";
#endif

#if (!DEBUG)
            host = "https://thuat.ebaocloud.com"
#endif
            return host;
        }
    }
}
