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
        public Boolean success = true;
        public string errorMessage;
        public Decimal totalFeeAmount;
        public Decimal totalTaxAmount;
        public Decimal payablePremium;
        public Decimal netPremium;
        public List<ItemDetail> feeDetails;
        public List<ItemDetail> taxDetails;

        public override string ToString()
        {
            if (!success)
            {
                return "\n\nCalculation Success: false\n" + "errorMessage: " + errorMessage;
            }
            else
            {
                String feeLog = "FeeDetails\n";
                foreach (ItemDetail feeDetail in feeDetails)
                {
                    feeLog = feeLog + feeDetail.ToString();
                }
                return "\n\nCalculation Success: true\n" + "TotalFeeAmount: " + totalFeeAmount + "\nTotalTaxAmount: " + totalTaxAmount + "\nPayablePremium: " + payablePremium + "\nNetPremium: " + netPremium + feeLog;
            }
        }
    }

    public class ItemDetail
    {
        public String name;
        public Decimal amount;

        public override string ToString()
        {
            return "Name: " + name + "\nAmount" + amount;
        }
    }


}
