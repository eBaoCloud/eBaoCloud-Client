using System;
using com.ebaocloud.client.thai.seg.cmi.pub;

namespace com.ebaocloud.client.thai.seg.cmi.response
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
