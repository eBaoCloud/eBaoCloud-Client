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

            JArray insureds = new JArray();
            map["insureds"] = insureds;
            JObject insured = new JObject();
            insureds.Add(insured);
            insured["ext"] = new JObject();
            insured["ext"]["vehicleCountry"] = "THA";
            insured["ext"]["vehicleGarageType"] = param.vehicleGarageType;
            insured["ext"] ["vehicleYear"] = param.vehicleModelYear;
            insured["ext"]["vehicleMake"] = param.vehicleMakeCode;
            insured["ext"]["vehicleModel"] = param.vehicleModelCode;
            insured["ext"]["vehicleRegYear"] = param.vehicleRegistrationYear;
            insured["ext"]["vehicleDesc"] = param.vehicleModelDescription;
            insured["ext"]["vehicleGroup"] = param.vehicleGroup;
            insured["ext"]["vehicleMarket"] = param.vehicleMarketValue;
            insured["ext"]["capacity"] = param.vehicleCapacity;
            insured["ext"]["vehicleCode"] = param.vehicleCode;
            insured["ext"]["numOfSeats"] = param.vehicleNumOfSeats;

            JObject responseObj = NetworkUtils.Post(ApiConsts.API_CALCULATE, JsonConvert.SerializeObject(map, Formatting.Indented),token);
            CalculationResp calcResp = new CalculationResp();
            calcResp.success = (Boolean)responseObj["success"];
            if (calcResp.success)
            {
                calcResp.totalFeeAmount = (Decimal)responseObj["data"]["premium"]["totalFeeAmount"];
                calcResp.payablePremium = (Decimal)responseObj["data"]["premium"]["app"];
                calcResp.totalTaxAmount = (Decimal)responseObj["data"]["premium"]["totalTaxAmount"];
                calcResp.netPremium = (Decimal)(Decimal)responseObj["data"]["premium"]["anp"];
                List<ItemDetail> feeDetails = new List<ItemDetail>();
                JArray feeArray = (JArray)responseObj["data"]["premium"]["feeAmounts"];
                foreach(JObject fee in feeArray)
                {
                    ItemDetail feeDetail = new ItemDetail();
                }
                calcResp.feeDetails = new Arr
            } else
            {
                calcResp.errorMessage = (String)responseObj["message"];
            }
            return calcResp;
        }

        string PolicyService.Issue(string token, Policy param)
        {
            if (String.IsNullOrEmpty(token)) throw new Exception("token is required");
            if (param == null) throw new Exception("parameter is required");
            JObject policy = buildPolicy(param);

            JObject responseObj = NetworkUtils.Post(ApiConsts.API_BUY, JsonConvert.SerializeObject(policy, Formatting.Indented), token);
            return "";
        }

        private static JObject buildPolicy(Policy param)
        {
            if (param == null) throw new Exception("param is required");
            JObject policy = new JObject();
            policy["insurerTenantCode"] = "SEG_TH";
            policy["effDate"] = Utils.FormatDate(param.effectiveDate);
            policy["expDate"] = Utils.FormatDate(param.expireDate);
            policy["prdtCode"] = param.productCode;
            policy["prdtVersion"] = param.productVersion;
            policy["policySource"] = 1;
            policy["proposalDate"] = Utils.FormatDate(param.proposalDate);
            policy["newOrRn"] = 1;

            policy["policyholder"] = buildPolicyholder(param);

            policy["ext"] = new JObject();
            policy["ext"]["planCode"] = "TIB";
            policy["ext"]["payer"] = buildPayer(param);
            policy["ext"]["dirverInfo"] = new JObject();
            policy["ext"]["dirverInfo"]["drivers"] = buildDriver(param);

            policy["insureds"] = buildInsured(param);

            return policy;
        }

        private static JArray buildInsured(Policy param)
        {
            if (param == null) throw new Exception("param is required");
            JArray insureds = new JArray();
            

            JObject insured = new JObject();
            insureds.Add(insured);
            insured["ext"] = new JObject();
           insured["ext"]["vehicleCountry"] = "THA";
           insured["ext"]["vehicleGarageType"] = param.insured.vehicleGarageType;
           insured["ext"]["vehicleProvince"] = param.insured.vehicleProvince;
           insured["ext"]["vehicleMake"] = param.insured.vehicleMake;
           insured["ext"]["vehicleModel"] = param.insured.vehicleModel;
           insured["ext"]["vehicleRegYear"] = param.insured.vehicleRegYear;
           insured["ext"]["vehicleDesc"] = param.insured.vehicleDesc;
           insured["ext"]["vehicleGroup"] = param.insured.vehicleGroup;
           insured["ext"]["vehicleMarket"] = param.insured.vehicleMarket;
           insured["ext"]["capacity"] = param.insured.vehicleCapacity;
           insured["ext"]["vehicleCode"] = param.insured.vehicleCode;
           insured["ext"]["numOfSeats"] = param.insured.vehicleNumOfSeats;
            insured["ext"]["vehicleChassisNo"] = param.insured.vehicleChassisNo;
            insured["ext"]["vehicleRegNo"] = param.insured.vehicleRegistrationNo;
            insured["ext"]["tonnage"] = param.insured.vehicleTonnage;



            return insureds;
        }

        private static JArray buildDriver(Policy param)
        {
            if (param == null) throw new Exception("param is required");
            JArray drivers = new JArray();
            if (param.drivers.Count() > 0)
            {
                foreach (Driver paramDriver in param.drivers)
                {
                    JObject driver = new JObject();
                    driver["name1"] = paramDriver.firstName;
                    driver["name3"] = paramDriver.lastName;
                    driver["profile"] = new JObject();
                    driver["profile"]["occupation"] = paramDriver.occupation;
                    driver["profile"]["dob"] = Utils.FormatDate(paramDriver.birthday);
                    drivers.Add(driver);
                }
            }
            return drivers;
        }

        private static JObject buildPayer(Policy param)
        {
            if (param == null) throw new Exception("param is required");
            JObject payer = new JObject();
            payer["name1"] = param.payer.name;
            payer["ext"] = new JObject();
            payer["ext"]["sameAsPolicyholder"] = param.isPayerSameAsPolicyholder;
            payer["ext"]["address"] = new JObject();
            if (param.payer.inThaiAddress != null)
            {
                payer["ext"]["address"]["addressType"] = 1;
                payer["ext"]["address"]["street"] = param.payer.inThaiAddress.street;
                payer["ext"]["address"]["province"] = param.payer.inThaiAddress.province;
                payer["ext"]["address"]["district"] = param.payer.inThaiAddress.district;
                payer["ext"]["address"]["subDistrict"] = param.payer.inThaiAddress.subDistrict;
                payer["ext"]["address"]["postalCode"] = param.payer.inThaiAddress.postalCode;
            }
            if (param.payer.outThaiAddress != null)
            {
                payer["ext"]["address"]["addressType"] = 0;
                payer["ext"]["address"]["address"] = param.payer.outThaiAddress.address;
                payer["ext"]["address"]["city"] = param.payer.outThaiAddress.city;
                payer["ext"]["address"]["country"] = param.payer.outThaiAddress.country;
            }
            return payer;
        }

        private static JObject buildPolicyholder(Policy param)
        {
            if (param == null) throw new Exception("param is required");
            JObject policyholder = new JObject();
            policyholder["indiOrOrg"] = param.indiPolicyholder == null ? 2 : 1;
            // TODO - indiOrOrg
            if (param.indiPolicyholder != null)
            {
                policyholder["taxNo"] = param.indiPolicyholder.taxNo;
                policyholder["name1"] = param.indiPolicyholder.firstName;
                policyholder["name3"] = param.indiPolicyholder.lastName;
                policyholder["idType"] = param.indiPolicyholder.idType;
                policyholder["idNo"] = param.indiPolicyholder.idNo;
                policyholder["title"] = (int)param.indiPolicyholder.title;

                policyholder["profile"] = new JObject();
                policyholder["profile"]["contactMobile"] = param.indiPolicyholder.mobile;

                policyholder["ext"] = new JObject();
                policyholder["ext"]["address"] = buildPolicyholderAddress(param);
            }
            if (param.orgPolicyhodler != null)
            {
                policyholder["taxNo"] = param.orgPolicyhodler.taxNo;
                policyholder["title"] = (int)param.orgPolicyhodler.title;
                policyholder["name1"] = param.orgPolicyhodler.companyName;
                policyholder["idNo"] = param.orgPolicyhodler.registrationNo;

                policyholder["ext"] = new JObject();
                policyholder["ext"]["branch"] = param.orgPolicyhodler.branch;
                policyholder["ext"]["address"] = buildPolicyholderAddress(param);
            }
            return policyholder;
        }

        private static JObject buildPolicyholderAddress(Policy param)
        {
            if (param == null) throw new Exception("param is required");
            JObject policyholderAddress = new JObject();
            if (param.indiPolicyholder.inThaiAddress == null && param.indiPolicyholder.outThaiAddress == null) throw new Exception("[Policyholder] InThaiAddress or OutThaiAddress is required");
            if (param.indiPolicyholder.inThaiAddress != null)
            {
                policyholderAddress["addressType"] = 1;
                policyholderAddress["street"] = param.indiPolicyholder.inThaiAddress.street;
                policyholderAddress["province"] = param.indiPolicyholder.inThaiAddress.province;
                policyholderAddress["district"] = param.indiPolicyholder.inThaiAddress.district;
                policyholderAddress["subDistrict"] = param.indiPolicyholder.inThaiAddress.subDistrict;
                policyholderAddress["postalCode"] = param.indiPolicyholder.inThaiAddress.postalCode;
            }
            if (param.indiPolicyholder.outThaiAddress != null)
            {
                policyholderAddress["addressType"] = 0;
                policyholderAddress["address"] = param.orgPolicyhodler.outThaiAddress.address;
                policyholderAddress["city"] = param.orgPolicyhodler.outThaiAddress.city;
                policyholderAddress["country"] = param.orgPolicyhodler.outThaiAddress.country;
            }
            return policyholderAddress;
        }
    }
}
