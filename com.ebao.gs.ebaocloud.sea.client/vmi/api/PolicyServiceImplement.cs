using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.seg.client.pub;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;
using EasyHttp.Http;
using Newtonsoft.Json;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.response;
using Newtonsoft.Json.Linq; 

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.api
{
    class PolicyServiceImplement : PolicyService
    {
        LoginResp PolicyService.Login(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new Exception("username is required");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new Exception("password is required");
            }
            
            JObject responseObj = NetworkUtils.Post(ApiConsts.API_LOGIN, null, Utils.Base64Encode(username + ":" + password));

            LoginResp loginResp = new LoginResp();
            loginResp.success = (Boolean)responseObj["success"];
            if (loginResp.success)
            {
                String passwd = (String)responseObj["data"]["user"]["password"];
                loginResp.token = Utils.Base64Encode(username + ":" + passwd); 
            }
            return loginResp;
        }

        CalculationResp PolicyService.Calculate(string token, CalculationParams param)
        {
            if (String.IsNullOrEmpty(token)) throw new Exception("token is required");
            if (param == null) throw new Exception("parameter is required");

            JObject map = new JObject();
            map["insurerTenantCode"] = "SEG_TH";
            map["effDate"] = Utils.FormatDate(param.effectiveDate);
            map["expDate"] = Utils.FormatDate(param.expireDate);
            map["prdtCode"] = param.productCode;
            map["prdtVersion"] = param.productVersion;
            map["policySource"] = 1;
            map["proposalDate"] = Utils.FormatDate(param.proposalDate);
            map["newOrRn"] = 1;

            map["ext"] = new JObject();
            map["ext"]["planCode"] = param.planCode;

            map["insureds"] = new JArray(1);
            map["insureds"][0] = new JObject();
            map["insureds"][0]["ext"] = new JObject();
            map["insureds"][0]["ext"]["vehicleCountry"] = "THA";
            map["insureds"][0]["ext"]["vehicleGarageType"] = param.vehicleGarageType;
            map["insureds"][0] ["ext"] ["vehicleYear"] = param.vehicleModelYear;
            map["insureds"][0]["ext"]["vehicleMake"] = param.vehicleMakeCode;
            map["insureds"][0]["ext"]["vehicleModel"] = param.vehicleModelCode;
            map["insureds"][0]["ext"]["vehicleRegYear"] = param.vehicleRegistrationYear;
            map["insureds"][0]["ext"]["vehicleDesc"] = param.vehicleModelDescription;
            map["insureds"][0]["ext"]["vehicleGroup"] = param.vehicleGroup;
            map["insureds"][0] ["ext"]["vehicleMarket"] = param.vehicleMarketValue;
            map["insureds"][0]["ext"]["capacity"] = param.vehicleCapacity;
            map["insureds"][0]["ext"]["vehicleCode"] = param.vehicleCode;
            map["insureds"][0]["ext"]["numOfSeats"] = param.vehicleNumOfSeats;

            JObject responseObj = NetworkUtils.Post(ApiConsts.API_CALCULATE, JsonConvert.SerializeObject(map, Formatting.Indented),token);
            CalculationResp calcResp = new CalculationResp();
            calcResp.success = (Boolean)responseObj["success"];
            if (calcResp.success)
            {
                calcResp.feeAmount = (Decimal)responseObj["premium"]["totalFeeAmount"];
                calcResp.premium = (Decimal)responseObj["premium"]["app"];
                calcResp.taxAmount = (Decimal)responseObj["premium"]["totalTaxAmount"];
            } else
            {
                calcResp.errorMessage = (String)responseObj["message"];
            }
            return calcResp;
        }

        string PolicyService.Issue(string token, Policy policy)
        {
            throw new NotImplementedException();
        }
    }
}
