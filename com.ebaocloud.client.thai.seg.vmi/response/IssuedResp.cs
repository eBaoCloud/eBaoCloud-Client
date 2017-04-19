using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebaocloud.client.thai.seg.vmi.pub;

namespace com.ebaocloud.client.thai.seg.vmi.response
{
    public class IssuedResp : BaseModel
    {
        public Boolean success = true;
        public string message { get; set; }
        public string policyNo { get; set; }
    }

}
