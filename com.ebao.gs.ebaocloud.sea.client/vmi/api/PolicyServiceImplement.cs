using System;
using System.Collections.Generic;
using System.Linq;
using com.ebao.gs.ebaocloud.sea.seg.client.pub;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;
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
            JObject calculationParams = buildCalculationParams(param);

            JObject responseObj = NetworkUtils.Post(ApiConsts.API_CALCULATE, calculationParams, token);
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
                foreach (JObject fee in feeArray)
                {
                    ItemDetail feeDetail = new ItemDetail();
                    feeDetail.name = (String)fee["name"];
                    feeDetail.amount = (Decimal)fee["amount"];
                    feeDetails.Add(feeDetail);
                }
                calcResp.feeDetails = feeDetails;

                List<ItemDetail> taxDetails = new List<ItemDetail>();
                JArray taxArray = (JArray)responseObj["data"]["premium"]["taxAmounts"];
                foreach (JObject tax in taxArray)
                {
                    ItemDetail taxDetail = new ItemDetail();
                    taxDetail.name = (String)tax["name"];
                    taxDetail.amount = (Decimal)tax["amount"];
                    taxDetails.Add(taxDetail);
                }
                calcResp.taxDetails = taxDetails;
            }
            else
            {
                calcResp.errorMessage = (String)responseObj["message"];
            }
            return calcResp;
        }

        private static JObject buildCalculationParams(CalculationParams param)
        {
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
            insured["ext"]["vehicleYear"] = param.vehicleModelYear;
            insured["ext"]["vehicleMake"] = param.vehicleMakeCode;
            insured["ext"]["vehicleModel"] = param.vehicleModelCode;
            insured["ext"]["vehicleRegYear"] = param.vehicleRegistrationYear;
            insured["ext"]["vehicleDesc"] = param.vehicleModelDescription;
            insured["ext"]["vehicleGroup"] = param.vehicleGroup;
            insured["ext"]["vehicleMarket"] = param.vehicleMarketValue;
            insured["ext"]["capacity"] = param.vehicleCapacity;
            insured["ext"]["vehicleCode"] = param.vehicleCode;
            insured["ext"]["numOfSeats"] = param.vehicleNumOfSeats;
            return map;
        }

        IssuedResp PolicyService.Issue(string token, Policy param)
        {
            if (String.IsNullOrEmpty(token)) throw new Exception("token is required");
            if (param == null) throw new Exception("policy is required");

            IssuedResp issuedResp = new IssuedResp();
            
            try
            {
                JObject policy = buildPolicy(token, param);

                JObject buyResult = NetworkUtils.Post(ApiConsts.API_BUY, policy, token);
                if(!parseResult(issuedResp, buyResult))
                {
                    return issuedResp;
                }

                JObject bindResult = NetworkUtils.Post(ApiConsts.API_BIND, buyResult["data"]["policy"], token);
                if (!parseResult(issuedResp, bindResult))
                {
                    return issuedResp;
                }

                String policyId = (String)bindResult["data"]["policy"]["policyId"];
                JObject confirmResult = NetworkUtils.Get(ApiConsts.API_CONFRIM + policyId, token);
                if (!parseResult(issuedResp, confirmResult))
                {
                    return issuedResp;
                }

                JObject payResult = NetworkUtils.Post(ApiConsts.API_PAY + policyId, buildPayMode(), token);
                if (!parseResult(issuedResp, payResult))
                {
                    return issuedResp;
                } 
            
                
                JObject paymentStatusResult = NetworkUtils.Get(ApiConsts.API_PAYMENT_STATUS + policyId, token);
                if (!parseResult(issuedResp, paymentStatusResult))
                {
                    return issuedResp;
                }
                else
                {
                    issuedResp.policyNo = (String)paymentStatusResult["data"]["policyNos"]["VMI"];
                    return issuedResp;
                }
            }
            catch (Exception e)
            {
                issuedResp.success = false;
                issuedResp.message = e.Message;
            }

            return issuedResp;
        }

        private static JObject buildPayMode()
        {
            JObject payMode = new JObject();
            payMode["payMode"] = "credit";
            return payMode;
        }

        private static Boolean parseResult(IssuedResp result, JObject responseObj)
        {
            result.success = (Boolean)responseObj["success"];
            if(!result.success)
            {
                result.message = (String)responseObj["message"];
                return false;
            } else
            {
                String processStatus = (String)responseObj["data"]["processStatus"];
                if("FAIL".Equals(processStatus) || "SUSPENDED".Equals(processStatus))
                {
                    result.success = false;
                    result.message = (String)responseObj["data"]["messages"][0]["message"];
                }
            }
            return true;
        }

        private static JObject buildPolicy(String token, Policy param)
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

            policy["insureds"] = buildInsured(token, param);
            policy["simpleFee"] = buildSimpleFee(token, param);

            return policy;
        }

        private static JObject buildSimpleFee(String token, Policy param)
        {

            PolicyService service = new PolicyServiceImplement();
            CalculationResp calcResp = service.Calculate(token, prepareCalculationParams(param));

            JObject simpleFee = new JObject();
            simpleFee["agp"] = calcResp.netPremium;
            simpleFee["snp"] = calcResp.netPremium;
            simpleFee["anp"] = calcResp.netPremium;
            simpleFee["app"] = calcResp.payablePremium;
            JArray taxDetails = new JArray();

            foreach (ItemDetail detail in calcResp.taxDetails)
            {
                JObject tax = new JObject();
                tax["name"] = detail.name;
                tax["amount"] = detail.amount;
                taxDetails.Add(tax);

            }
            simpleFee["taxAmounts"] = taxDetails;

            JArray feeDetails = new JArray();

            foreach (ItemDetail detail in calcResp.feeDetails)
            {
                JObject fee = new JObject();
                fee["name"] = detail.name;
                fee["amount"] = detail.amount;
                feeDetails.Add(fee);
            }
            simpleFee["feeAmounts"] = feeDetails;

            return simpleFee;
        }

        private static JObject getVechileModel(String token, String makeName, int modelYear, String modelDescription)
        {
            if (String.IsNullOrEmpty(token)) throw new Exception("Token is required");
            if (String.IsNullOrEmpty(makeName)) throw new Exception("MakeNamke is required");
            if (modelYear == 0) throw new Exception("ModelYear is required");
            if (String.IsNullOrEmpty(modelDescription)) throw new Exception("ModelDescription is required");


        }

        private static CalculationParams prepareCalculationParams(Policy param)
        {
            CalculationParams calculationParams = new CalculationParams();
            calculationParams.effectiveDate = param.effectiveDate;
            calculationParams.expireDate = param.expireDate;
            calculationParams.planCode = param.planCode;
            calculationParams.productCode = param.productCode;
            calculationParams.productVersion = param.productVersion;
            calculationParams.proposalDate = param.proposalDate;




            calculationParams.vehicleAccessaryValue = param.insured.vehicleAccessaryValue;
            calculationParams.vehicleCapacity = param.insured.vehicleCapacity;
            calculationParams.vehicleCode = param.insured.vehicleCode;
            calculationParams.vehicleGarageType = param.insured.vehicleGarageType;
            calculationParams.vehicleGroup = param.insured.vehicleGroup;
            calculationParams.vehicleMakeCode = param.insured.vehicleMake;
            calculationParams.vehicleMarketValue = param.insured.vehicleMarket;
            calculationParams.vehicleModelCode = param.insured.vehicleModel;
            calculationParams.vehicleModelDescription = param.insured.vehicleDesc;
            calculationParams.vehicleModelYear = param.insured.vehicleYear;
            calculationParams.vehicleNumOfSeats = param.insured.vehicleNumOfSeats;
            calculationParams.vehicleRegistrationYear = param.insured.vehicleRegYear;
            calculationParams.vehicleTonnage = param.insured.vehicleTonnage;
            calculationParams.vehicleTotalValue = param.insured.vehicleTotalValue;




            return calculationParams;
        }

        private static JArray buildCoverages(String token, Policy param)
        {
            JObject value = buildCalculationParams(prepareCalculationParams(param));
            JObject responseObj = NetworkUtils.Post(ApiConsts.API_COVERAGES,value , token);
            Boolean result = (Boolean)responseObj["success"];
            if(result)
            {
                return (JArray)responseObj["data"];

            } else
            {
                throw new Exception((String)responseObj["message"]);
            }
        }

        private static JArray buildInsured(String token, Policy param)
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

            insured["coverages"] = buildCoverages(token, param);

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
            // TODO - isPayerSameAsPolicyholder
            if (param == null) throw new Exception("param is required");
            JObject payer = new JObject();
            payer["name1"] = param.payer.name;
            payer["indiOrOrg"] = 1;
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
                payer["ext"]["address"]["cascadeAddress"] = param.payer.inThaiAddress.subDistrict;
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
            //TODO indiOrOrg
            if (param.indiPolicyholder == null && param.orgPolicyhodler == null) throw new Exception("PolicyHolder is required");
            if (param.indiPolicyholder != null)
            {
                policyholder["indiOrOrg"] = 1;
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
                policyholder["indiOrOrg"] = 2;
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
                policyholderAddress["cascadeAddress"] = param.indiPolicyholder.inThaiAddress.subDistrict;
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

		private static JArray buildPolicyDocument(Policy param, long policyId)
		{
			if (policyId == 0L) throw new Exception("Policy id is required");

			JArray documents = new JArray();
			if (param.documents.Count() > 0)
			{
				foreach (Document document in param.documents)
				{
					//TODO category and name is required
					if (!document.file.Exists)
					{
						throw new Exception("The file [" + document.file.FullName + "] does not exists.");
					}
					UploadFileParams uploadFileParams = new UploadFileParams();
					uploadFileParams.fileInfo = document.file;

					uploadFileParams.uploadExtraData = new JObject();
					uploadFileParams.uploadExtraData["policyId"] = policyId;
					uploadFileParams.uploadExtraData["docName"] = document.name;
					uploadFileParams.uploadExtraData["docType"] = document.category;
					documents.Add(uploadFileParams);
				}
			}
			return documents;
		}
	}
}
