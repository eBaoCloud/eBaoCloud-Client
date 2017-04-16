using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using com.ebao.gs.ebaocloud.sea.client.Model.PolicyModel;

namespace com.ebao.gs.ebaocloud.sea.client.Model.PremCalcResultModel
{
    abstract class BasePolicyServiceResult
    {
        string processStatus;
        List<MessageModel> messages;
    }

    class PremCalcResult : BasePolicyServiceResult
    {
        Policy policy;
        Boolean uwPassed = true;
        List<SimpleFee> premiums;
    }

}
