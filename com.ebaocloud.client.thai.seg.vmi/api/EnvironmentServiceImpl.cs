﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.ebaocloud.client.thai.seg.vmi.pub;

namespace com.ebaocloud.client.thai.seg.vmi.api
{
    public class EnvironmentServiceImpl : EnvironmentService
    {
        public void SetEnvironmentType(EnvironmentType type)
        {
            ApiServiceFactory.SetCurrentHost(type);
        }
    }
}
