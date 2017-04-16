using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.client.Model.PolicyModel;

namespace com.ebao.gs.ebaocloud.sea.client.Model.Params.Product
{
    class CMIParam : BaseMotorParam
    {
        String policySource;
        String phType;
        DateTime policyEffDate;
        DateTime policyExpDate;
        List<Insured> insureds;

    }
}
