using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.response;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub;
using System.IO;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.api
{
	public class PolicyServiceImplement : PolicyService
	{
		public LoginResp Login(string username, string password)
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

		public CalculationResp Calculate(string token, CalculationParams param)
		{
			if (String.IsNullOrEmpty(token)) throw new Exception("token is required");
			if (param == null) throw new Exception("parameter is required");
			JObject calculationParams = buildCalculationParams(token, param);

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

		public IssuedResp Issue(string token, Policy param)
		{
			if (String.IsNullOrEmpty(token)) throw new Exception("token is required");
			if (param == null) throw new Exception("policy is required");
			IssuedResp issuedResp = new IssuedResp();
			try
			{
				JObject policy = buildPolicy(token,param);

				//do get product structure
				JObject structureResult = NetworkUtils.Post(ApiConsts.API_STRUCTURE, policy, token);

				if (!parseResult(issuedResp,structureResult)) {
					return issuedResp;
				}
				policy["ext"]["pd"] = structureResult["data"];
				policy["ext"]["ui"]["step"] = 1;
				//do buy operation
				JObject buyResult = NetworkUtils.Post(ApiConsts.API_BUY, policy, token);
				if (!parseResult(issuedResp, buyResult))
				{
					return issuedResp;
				}
				policy["ext"]["ui"]["step"] = 2;
				//do bind operation
				JObject bindResult = NetworkUtils.Post(ApiConsts.API_BIND, buyResult["data"]["policy"], token);
				if (!parseResult(issuedResp, bindResult))
				{
					return issuedResp;
				}
				
				policy["ext"]["ui"]["step"] = 3;
				long policyId = (long)bindResult["data"]["policy"]["policyId"];

				uploadPolicyDocument(param, policyId, token);

				//do confirm operation
				JObject confirmResult = NetworkUtils.Get(ApiConsts.API_CONFRIM + policyId, token);
				if (!parseResult(issuedResp, confirmResult))
				{
					return issuedResp;
				}
				policy["ext"]["ui"]["step"] = 4;
				
				//do payment operation
				JObject payResult = NetworkUtils.Post(ApiConsts.API_PAY + policyId, buildPayMode(), token);
				if (!parseResult(issuedResp, payResult))
				{
					return issuedResp;
				}
			
				//do issue operation
				JObject paymentStatusResult = NetworkUtils.Get(ApiConsts.API_PAYMENT_STATUS + policyId, token);
				if (!parseResult(issuedResp, paymentStatusResult))
				{
					return issuedResp;
				}
				else
				{
					issuedResp.policyNo = (String)paymentStatusResult["data"]["policyNos"]["CMI"];
					return issuedResp;
				}
			}
			catch (Exception e) {
				issuedResp.success = false;
				issuedResp.message = e.Message;
			}
			return issuedResp;
		}

		//build policy basic info
		private static JObject buildPolicy(string token,Policy param) {
			JObject policy = new JObject();
			if (param == null) throw new Exception("param is required");
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
			policy["ext"]["payer"] = buildPayer(param);

			policy["ext"]["ui"] = new JObject();

			policy["insureds"] = buildInsured(token, param);
			policy["simpleFee"] = buildSimpleFee(token, param);
			return policy;
		}

		private static JArray buildInsured(String token, Policy param)
		{
			if (param == null) throw new Exception("param is required");

			JObject vehicle = getVechileModel(token, param.insured.vehicleMakeName, param.insured.vehicleModelYear, param.insured.vehicleModelDescription);

			JArray insureds = new JArray();
			JObject insured = new JObject();
			insured["ext"] = new JObject();
			insured["ext"]["vehicleMake"] = vehicle["makeCode"];
			insured["ext"]["vehicleModel"] = vehicle["modelCode"];
			insured["ext"]["vehicleGroup"] = vehicle["vehicleGroup"];
			insured["ext"]["vehicleMarket"] = vehicle["marketPrice"];
			insured["ext"]["capacity"] = vehicle["capacity"];
			insured["ext"]["numOfSeats"] = vehicle["numOfSeat"];
			insured["ext"]["tonnage"] = vehicle["tonnage"];

			insured["ext"]["vehicleDesc"] = param.insured.vehicleModelDescription;

			insured["ext"]["vehicleType"] = Convert.ToString((int)param.insured.vehicleType);
			insured["ext"]["vehicleSubType"] = Convert.ToString((int)param.insured.vehicleSubType);
			insured["ext"]["vehicleUsage"] = Utils.ToVehicleUsage(param.insured.vehicleUsage);
			insured["ext"]["vehicleCode"] = Utils.ToVehicleCode(param.insured.vehicleUsage);
			insured["ext"]["vehicleChassisNo"] = param.insured.vehicleChassisNo;

			insured["ext"]["vehicleCountry"] = String.IsNullOrEmpty(param.insured.vehicleCountry) ? "THA" : param.insured.vehicleCountry;
			var regYear = param.insured.vehicleRegistrationYear;
			if (Convert.ToString(regYear).Equals(DateTime.Now.ToString("yyyy")))
			{
				//new vehicle
				insured["ext"]["newVehicle"] = true;
				insured["ext"]["vehicleProvince"] = "TBA";
				insured["ext"]["vehicleRegYear"] = int.Parse(DateTime.Now.ToString("yyyy"));
				insured["ext"]["vehicleRegNo"] = "TBA";
			}
			else {
				insured["ext"]["newVehicle"] = false;
				insured["ext"]["vehicleProvince"] = param.insured.vehicleProvince;
				insured["ext"]["vehicleRegYear"] = regYear;
				insured["ext"]["vehicleRegNo"] = param.insured.vehicleRegistrationNo;
			}
			insured["coverages"] = buildCoverages(token, prepareCalculationParams(param));
			insureds.Add(insured);
			return insureds;
		}

		private static JArray buildCoverages(String token, CalculationParams param)
		{
			return buildCoverages(token, buildCalculationParams(token, param));
		}

		private static JArray buildCoverages(String token, JObject param)
		{
			JObject responseObj = NetworkUtils.Post(ApiConsts.API_COVERAGES, param, token);
			Boolean result = (Boolean)responseObj["success"];
			if (result)
			{
				return (JArray)responseObj["data"];

			}
			else
			{
				throw new Exception((String)responseObj["message"]);
			}
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

		private static JObject buildCalculationParams(String token, CalculationParams param)
		{
			JObject map = new JObject();
			map["insurerTenantCode"] = "SEG_TH";
			map["effDate"] = Utils.FormatDate(param.effectiveDate);
			map["expDate"] = Utils.FormatDate(param.expireDate);
			map["prdtCode"] = param.productCode;
			map["policySource"] = 1;
			map["proposalDate"] = Utils.FormatDate(param.proposalDate);
			map["newOrRn"] = 1;

			JArray insureds = new JArray();
			map["insureds"] = insureds;
			JObject insured = new JObject();
			insureds.Add(insured);
			insured["ext"] = new JObject();
			insured["ext"]["vehicleCode"] = Utils.ToVehicleCode(param.vehicleUsage);
			return map;
		}

		private static JObject buildPayMode()
		{
			JObject payMode = new JObject();
			payMode["payMode"] = "credit";
			return payMode;
		}

		private static JObject getVechileModel(String token, String makeName, int modelYear, String modelDescription)
		{
			if (String.IsNullOrEmpty(token)) throw new Exception("Token is required");
			if (String.IsNullOrEmpty(makeName)) throw new Exception("Vehicle make name is required");
			if (modelYear == 0) throw new Exception("Vehicle model year is required");
			if (String.IsNullOrEmpty(modelDescription)) throw new Exception("Vehicle model description is required");

			JObject queryParams = new JObject();
			queryParams["makeName"] = makeName;
			queryParams["modelYear"] = modelYear;
			queryParams["subModelName"] = modelDescription;
			queryParams["insurerTenantCode"] = "SEG_TH";
			JObject responseObj = NetworkUtils.Post(ApiConsts.API_VEHICLE, queryParams, token);
			if ((Boolean)responseObj["success"])
			{
				if (responseObj["data"] == null)
				{
					throw new Exception("Cannot fetch a vehicle model.");
				}
				return (JObject)responseObj["data"];
			}
			else
			{
				throw new Exception("Cannot fetch a vehicle model.");
			}
		}

		private static CalculationParams prepareCalculationParams(Policy param)
		{
			CalculationParams calculationParams = new CalculationParams();
			calculationParams.effectiveDate = param.effectiveDate;
			calculationParams.expireDate = param.expireDate;
			calculationParams.productCode = param.productCode;
			calculationParams.proposalDate = param.proposalDate;

			calculationParams.vehicleUsage = param.insured.vehicleUsage;

			return calculationParams;
		}

		private static void uploadPolicyDocument(Policy param, long policyId, String token)
		{
			if (policyId == 0L) throw new Exception("Policy id is required");
			if (param.documents.Count() > 0)
			{
				foreach (Document document in param.documents)
				{
					//TODO category and name is required
					if (!document.file.Exists)
					{
						throw new Exception("The file [" + document.file.FullName + "] does not exists.");
					}
					if (document.category == 0)
					{
						throw new Exception("Document cate is required.");
					}
					if (String.IsNullOrEmpty(document.name)){
						throw new Exception("Document name is required.");
					}
					UploadFileParams uploadFileParams = new UploadFileParams();
					uploadFileParams.fileInfo = document.file;

					uploadFileParams.uploadExtraData = new JObject();
					uploadFileParams.uploadExtraData["policyId"] = policyId;
					uploadFileParams.uploadExtraData["docName"] = document.name;
					uploadFileParams.uploadExtraData["docType"] = (int)document.category;

					NetworkUtils.UploadFile(ApiConsts.API_DOCS, uploadFileParams, token);
				}
			}
		}

		public void Download(string token,string policyNo, string filePath)
		{
			if (String.IsNullOrEmpty(policyNo)) throw new Exception("Policy No. is required");
			FileInfo fileInfo = new FileInfo(filePath);
			if (fileInfo.Attributes != FileAttributes.Directory) {
				//check it is directory
				throw new Exception("The path [" + filePath + " must be directory.]");
			}if (!Directory.Exists(filePath)) {
				Directory.CreateDirectory(filePath); //if file does not exists, then create
			}

			JObject responseObj = NetworkUtils.Get(ApiConsts.API_GET_PRINTED_FILES + "/" + policyNo + "/CMI",token);
			JArray printedFileList = (JArray)responseObj["data"];
			if (responseObj["data"] != null)
			{
				foreach (string fileName in printedFileList)
				{
					NetworkUtils.download(ApiConsts.API_DOWNLOAD_PRINTED_FILE + "/?fileName=" + fileName, filePath, token);
				}
			}
		}

		private static Boolean parseResult(IssuedResp result, JObject responseObj)
		{
			result.success = (Boolean)responseObj["success"];
			if (!result.success)
			{
				result.message = (String)responseObj["message"];
				return false;
			}
			else
			{
				String processStatus = (String)responseObj["data"]["processStatus"];
				if ("FAIL".Equals(processStatus) || "SUSPENDED".Equals(processStatus))
				{
					result.success = false;
					result.message = (String)responseObj["data"]["messages"][0]["message"];
				}
			}
			return true;
		}

	}
}
