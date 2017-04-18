using System;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.response
{
	public class IssuedResp : BaseModel
	{
		public Boolean success = true;
		public string message { get; set; }
		public string policyNo { get; set; }

        public override string ToString()
        {
            return base.ToString(); 
        }
    }
}
