using System;
using System.Collections.Generic;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.api;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.response;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client
{
	public class CMITest
	{
		public static void Main(String[] args)
		{
			PolicyService service = new PolicyServiceImplement();
			LoginResp resp = service.Login("SEG_TIB_01", "eBao1234");

			var calculationParams = new CalculationParams();
			calculationParams.effectiveDate = DateTime.Now.ToLocalTime();
			calculationParams.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
			calculationParams.proposalDate = DateTime.Now.ToLocalTime();

			calculationParams.productCode = "CMI";
			calculationParams.vehicleUsage = VehicleUsage.PRIVATE;

			CalculationResp calcResp = service.Calculate(resp.token, calculationParams);

			Policy policyParam = new Policy();
			List<Document> documents = new List<Document>();
			Document doc = new Document();
			doc.category = DocumentCategory.DRIVING_LICENSE;
			doc.name = "test";
			doc.file = new System.IO.FileInfo("./Main.cs");
			documents.Add(doc);
			policyParam.documents = documents;
			policyParam.effectiveDate = DateTime.Now.ToLocalTime();
			policyParam.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
			policyParam.proposalDate = DateTime.Now.ToLocalTime();
			policyParam.productCode = "CMI";
			policyParam.productVersion = "v1";
			policyParam.isPayerSameAsPolicyholder = true;

			policyParam.insured = new Insured();
			policyParam.insured.vehicleChassisNo = "CN09876777777665";
			policyParam.insured.vehicleType = Convert.ToString(VehicleType.Sedan);
			policyParam.insured.vehicleSubType = Convert.ToString(VehicleSubType.Car_Seat_up_to_7_people);
			policyParam.insured.vehicleColor = "white";
			policyParam.insured.vehicleCountry = "THA";
			policyParam.insured.vehicleModelDescription = "Sedan 4dr G  6sp FWD 2.5 2016";
			policyParam.insured.vehicleMakeName = "TOYOTA";
			policyParam.insured.vehicleProvince = "THA";
			policyParam.insured.vehicleRegistrationNo = "CN06667724442552F";
			policyParam.insured.vehicleRegistrationYear = 2016;
			policyParam.insured.vehicleUsage = VehicleUsage.PRIVATE;
			policyParam.insured.vehicleModelYear = 2016;



			policyParam.payer = new Payer();
			policyParam.payer.inThaiAddress = new InThaiAddress();
			policyParam.payer.inThaiAddress.district = "1001";
			policyParam.payer.inThaiAddress.postalCode = "10200";
			policyParam.payer.inThaiAddress.province = "10";
			policyParam.payer.inThaiAddress.street = "songhu rd.";
			policyParam.payer.inThaiAddress.subDistrict = "100101";
			policyParam.payer.name = "Jacky Cheng";

			policyParam.indiPolicyholder = new IndividualPolicyholder();
			policyParam.indiPolicyholder.idNo = "123456";
			policyParam.indiPolicyholder.idType = "1";
			policyParam.indiPolicyholder.inThaiAddress = new InThaiAddress();
			policyParam.indiPolicyholder.inThaiAddress.district = "1001";
			policyParam.indiPolicyholder.inThaiAddress.postalCode = "10200";
			policyParam.indiPolicyholder.inThaiAddress.province = "10";
			policyParam.indiPolicyholder.inThaiAddress.street = "songhu rd.";
			policyParam.indiPolicyholder.inThaiAddress.subDistrict = "100101";
			policyParam.indiPolicyholder.lastName = "Cheng";
			policyParam.indiPolicyholder.firstName = "Jacky";
			policyParam.indiPolicyholder.mobile = "1234999";
			policyParam.indiPolicyholder.taxNo = "10000";
			policyParam.indiPolicyholder.title = IndividualPrefix.Khun;

			IssuedResp hi = service.Issue(resp.token, policyParam);
		}
	}
}
