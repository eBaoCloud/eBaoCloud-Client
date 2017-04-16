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
        public Decimal feeAmount;
        public Decimal taxAmount;
        public Decimal premium;
    }
}
