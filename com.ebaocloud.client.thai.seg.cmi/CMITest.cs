using System;
using System.Collections.Generic;
using com.ebaocloud.client.thai.seg.cmi.api;
using com.ebaocloud.client.thai.seg.cmi.parameters;
using com.ebaocloud.client.thai.seg.cmi.response;

namespace com.ebaocloud.client.thai.seg.cmi
{
    public class CMITest
    {
        public static void Main(String[] args)
        {
            PolicyService service = new PolicyServiceImpl();
            LoginResp resp = service.Login("SEG_TIB_01", "eBao1234");

            MasterDataService masterDataService = new MasterDataServiceImpl();
            List<KeyValue> vehicleTypes = masterDataService.GetVehicleType();
            List<CascadeValue> vehicleSubTypes = masterDataService.GetVehicleSubType(vehicleTypes[0].key);
            List<KeyValue> vehicleUsages = masterDataService.GetVehicleUsage(vehicleSubTypes[0].key);

            var calculationParams = new CalculationParams();
            calculationParams.effectiveDate = DateTime.Now.ToLocalTime();
            calculationParams.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
            calculationParams.proposalDate = DateTime.Now.ToLocalTime();
            calculationParams.productCode = "CMI";
            calculationParams.vehicleUsage = vehicleUsages[0].key;
            CalculationResp calcResp = service.Calculate(resp.token, calculationParams);

            Policy policyParam = new Policy();
            List<Document> documents = new List<Document>();
            Document doc = new Document();
            doc.documentType = "2";
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

            String randomStr = new Random(DateTime.Now.Millisecond).Next().ToString();
            policyParam.insured = new Insured();
            policyParam.insured.vehicleChassisNo = "CN" + randomStr;
            policyParam.insured.vehicleRegistrationNo = "CN" + randomStr;

            List<CascadeValue> subTypes = masterDataService.GetVehicleSubType(vehicleTypes[0].key);
            policyParam.insured.vehicleType = vehicleTypes[0].key;
            policyParam.insured.vehicleSubType = subTypes[0].key;



            policyParam.insured.vehicleColor = "white";
            policyParam.insured.vehicleCountry = "THA";
            policyParam.insured.vehicleModelDescription = "Sedan 4dr G  6sp FWD 2.5 2016";
            policyParam.insured.vehicleMakeName = "TOYOTA";
            policyParam.insured.vehicleProvince = "THA";
            policyParam.insured.vehicleRegistrationYear = 2016;
            policyParam.insured.vehicleUsage = vehicleUsages[0].key;
            policyParam.insured.vehicleModelYear = 2016;

            policyParam.payer = new Payer();
            policyParam.payer.inThaiAddress = new InThaiAddress();
            policyParam.payer.inThaiAddress.district = "1001";
            policyParam.payer.inThaiAddress.postalCode = "10200";
            policyParam.payer.inThaiAddress.province = "10";
            policyParam.payer.inThaiAddress.street = "songhu rd.";
            policyParam.payer.inThaiAddress.subDistrict = "100101";
            policyParam.payer.name = "Jacky Cheng";
            policyParam.payer.inThaiAddress.fullAddress = "24 (318 เดิม) ซ.อุดมสุข30 แยก2 ถ.อุดมสุข แขวงบางนา เขตบางนา กทม. 10260";

            policyParam.indiPolicyholder = new IndividualPolicyholder();
            policyParam.indiPolicyholder.idNo = "123456";
            policyParam.indiPolicyholder.idType = "1";
            policyParam.indiPolicyholder.inThaiAddress = new InThaiAddress();
            policyParam.indiPolicyholder.inThaiAddress.district = "1001";
            policyParam.indiPolicyholder.inThaiAddress.postalCode = "10200";
            policyParam.indiPolicyholder.inThaiAddress.province = "10";
            policyParam.indiPolicyholder.inThaiAddress.street = "songhu rd.";
            policyParam.indiPolicyholder.inThaiAddress.subDistrict = "100101";
            policyParam.indiPolicyholder.inThaiAddress.fullAddress = "24 (318 เดิม) ซ.อุดมสุข30 แยก2 ถ.อุดมสุข แขวงบางนา เขตบางนา กทม. 10260";
            policyParam.indiPolicyholder.lastName = "luo";
            policyParam.indiPolicyholder.firstName = "leon";
            policyParam.indiPolicyholder.mobile = "1234999";
            policyParam.indiPolicyholder.taxNo = "10000";
            policyParam.indiPolicyholder.title = IndividualPrefix.Khun;

            IssuedResp issueResp = service.Issue(resp.token, policyParam);
            Console.WriteLine(issueResp);
            // service.Download(resp.token, "00000252", "D:/Private");
            Console.WriteLine();

            service.DownloadPolicyForms(resp.token, issueResp.policyNo, new System.IO.DirectoryInfo("C:/OutputFiles"));
        }
    }
}
