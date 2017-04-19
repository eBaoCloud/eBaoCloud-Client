using System;

namespace com.ebaocloud.client.thai.seg.cmi.response
{
	public class LoginResp
	{
		public Boolean success = true;
		public string token;

        public override string ToString()
        {
            return "\n\nLogin Success: " + success.ToString() + "\nLogin Token: " + token + "\n Tips: You should record the TOKEN in order to call the other APIs.";
        }
    }
}
