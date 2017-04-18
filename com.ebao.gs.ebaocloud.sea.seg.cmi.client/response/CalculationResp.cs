using System;
using System.Collections.Generic;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.response
{
	/// <summary>
	/// Calculation Result
	/// </summary>
	public class CalculationResp
	{
		public Boolean success = true;
		public string errorMessage { get; set; }
		public Decimal totalFeeAmount { get; set; }
        public Decimal totalTaxAmount { get; set; }
        public Decimal payablePremium { get; set; }
        public Decimal netPremium { get; set; }
        public List<ItemDetail> feeDetails { get; set; }
        public List<ItemDetail> taxDetails { get; set; }

    }

	public class ItemDetail
	{
		public String name { get; set; }
        public Decimal amount { get; set; }
    }
}
