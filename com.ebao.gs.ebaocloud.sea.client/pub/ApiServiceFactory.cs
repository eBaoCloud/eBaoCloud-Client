﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebao.gs.ebaocloud.sea.seg.client.pub
{
    public class ApiServiceFactory
    {
        private static string host = "https://th.ebaocloud.com";
        public static string getCurrentHost()
        {
#if DEBUG
            host = "http://172.18.49.49:8888";
#endif

#if (!DEBUG)
            host = "https://th.ebaocloud.com"
#endif
            return host;
        }
    }
}