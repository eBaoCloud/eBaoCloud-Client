using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.response;
using com.ebao.gs.ebaocloud.sea.seg.client.cmi.pub;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub;

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

		public IssuedResp Issue(string token, Policy policy)
		{
			throw new NotImplementedException();
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
			JObject responseObj = NetworkUtils.Post(ApiConsts.API_VEHICLE, queryParams, token);
			if ((Boolean)responseObj["success"])
			{
				return (JObject)responseObj["data"];
			}
			else
			{
				throw new Exception("Cannot fetch a vehicle model.");
			}
		}

	}
}
