using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			var calculationParams = new CalculationParams();
			calculationParams.effectiveDate = DateTime.Now.ToLocalTime();
			calculationParams.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
			calculationParams.proposalDate = DateTime.Now.ToLocalTime();

			calculationParams.productCode = "VMI";
			calculationParams.vehicleUsage = VehicleUsage.PRIVATE;

			CalculationResp calcResp = service.Calculate(resp.token, calculationParams);

			Console.WriteLine("{0}", calcResp);
		}
	}
}
