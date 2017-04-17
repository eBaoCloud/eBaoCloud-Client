using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters
{
	public class CalculationParams
	{
		public String productCode;
		public DateTime proposalDate;
		public DateTime effectiveDate;
		public DateTime expireDate;

		public VehicleUsage vehicleUsage; // as vehicle code

	}

	public enum VehicleUsage
	{
		PRIVATE = 11,
		RENT = 16,
		PUBLIC_RENT = 17
	}

	public enum VehicleCode {
		PRIVATE,
		RENT,
		PUBLIC_RENT
	}
}
