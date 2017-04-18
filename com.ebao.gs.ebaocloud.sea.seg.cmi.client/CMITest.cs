using System;
using System.Collections.Generic;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.api;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.response;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client
{
	public class CMITest
	{
		public static void Main(String[] args)
		{
			PolicyService service = new PolicyServiceImplement();
			LoginResp resp = service.Login("SEG_TIB_01", "eBao1234");

			service.Download(resp.token, "00000252", "D:/Private");
		}
	}
}
