using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebaocloud.client.thai.seg.vmi.pub
{
    public class ApiServiceFactory
    {
        private static string host = "";

        public static void SetCurrentHost(EnvironmentType type)
        {
            switch (type)
            {
                case EnvironmentType.TEST:
                    host = "https://thtst.ebaocloud.com";
                    break;
                case EnvironmentType.UAT:
                    host = "https://thuat.ebaocloud.com";
                    break;
                case EnvironmentType.PROD:
                    host = "https://th.ebaocloud.com";
                    break;
                case EnvironmentType.DEV:
                    host = "http://172.18.49.49:8888";
                    break;
            }
        }

        public static string getCurrentHost()
        {
            if (host == null || host == "")
            {
                throw new Exception("You must specify a environmentType.");
            }
            return host;
        }
    }

    public enum EnvironmentType
    {
        TEST = 1,
        UAT = 2,
        PROD = 3,
        DEV = 4
    }
}
