using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebaocloud.client.thai.seg.vmi.pub;

namespace com.ebaocloud.client.thai.seg.vmi.response
{
    /// <summary>
    /// Calculation Result
    /// </summary>
    public class CalculationResp : BaseModel
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

    public class ItemDetail : BaseModel
    {
        public String name { get; set; }
        public Decimal amount { get; set; }

        public override string ToString()
        {
            return "Name: " + name + "\nAmount: " + amount + "\n";
        }
    }


}
