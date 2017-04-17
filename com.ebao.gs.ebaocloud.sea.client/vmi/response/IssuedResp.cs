using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.seg.client.pub;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.response
{
    public class IssuedResp : BaseModel
    {
        /// <summary>
        /// response success
        /// </summary>
        public Boolean success = true;
        public string message { get; set; }
        public string policyNo { get; set; }
    }
}
