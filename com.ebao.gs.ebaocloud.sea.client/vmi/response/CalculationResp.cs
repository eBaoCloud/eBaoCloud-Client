using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.response
{
    /// <summary>
    /// Calculation Result
    /// </summary>
   public class CalculationResp
    {
        public Boolean success;
        public string errorMessage;
        public Decimal totalFeeAmount;
        public Decimal totalTaxAmount;
        public Decimal payablePremium;
        public Decimal netPremium;
        public List<ItemDetail> feeDetails;
        public List<ItemDetail> taxDetails;

    }

    public class ItemDetail
    {
        public String name;
        public Decimal amount;
    }


}
