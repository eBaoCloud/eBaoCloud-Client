using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Http;
using Newtonsoft.Json.Linq;
using System.Dynamic;

using com.ebaocloud.client.thai.seg.vmi.api;
using com.ebaocloud.client.thai.seg.vmi.parameters;
using com.ebaocloud.client.thai.seg.vmi.response;
using com.ebaocloud.client.thai.seg.vmi.pub;

namespace com.ebaocloud.client.thai.seg.vmi
{
    public class TestNetwork
    {
        public static void Main(string[] args)
        {
            PolicyService service = new PolicyServiceImpl();
           LoginResp resp = service.Login("TIB_01", "Seg@1234");
            Console.WriteLine(resp);

            var calculationParams = new CalculationParams();
            calculationParams.effectiveDate = DateTime.Now.ToLocalTime();
            calculationParams.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
            calculationParams.proposalDate = DateTime.Now.ToLocalTime();

           calculationParams.planCode = "SCDG";
            calculationParams.productCode = "VMI";
            calculationParams.vehicleAccessaryValue = 1000;
            calculationParams.vehicleGarageType = VehicleGarageType.DEALER;
           calculationParams.vehicleMakeName = "TOYOTA";
            calculationParams.vehicleModelName = "COROLLA";
            calculationParams.vehicleModelDescription = "TOYO20160104";
            calculationParams.vehicleModelYear = 2016;
            calculationParams.vehicleRegistrationYear = 2016;
            calculationParams.vehicleUsage = VehicleUsage.PRIVATE;
            //Console.WriteLine(calculationParams);
            CalculationResp calcResp = service.Calculate(resp.token, calculationParams);
            Console.WriteLine(calcResp);

            //LoginResp loginResp = service.Login("SEG_TIB_01", "eBao1234");

            Policy policyParam = new Policy();

            // Document(List) Upload
            List<Document> documents = new List<Document>();
            Document doc = new Document();
            doc.category = DocumentCategory.DRIVING_LICENSE;
            doc.name = "test";
            doc.file = new System.IO.FileInfo("../../UploadSample.txt");
            documents.Add(doc);
            policyParam.documents = documents;

            policyParam.effectiveDate = DateTime.Now.ToLocalTime();
            policyParam.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
            policyParam.proposalDate = DateTime.Now.ToLocalTime();
            policyParam.productCode = "VMI";
            policyParam.planCode = "SCGG";
            //policyParam.productVersion = "v1";
            // isPayerSameAsPolicyholder 
            // category - true / false
            // if yes, the payer address will be consistance with policyHolder.
            policyParam.isPayerSameAsPolicyholder = true;

            String randomStr = new Random(DateTime.Now.Millisecond).Next().ToString();
            policyParam.insured = new Insured();
            policyParam.insured.vehicleChassisNo = "CN" + randomStr;
            policyParam.insured.vehicleColor = "white";
            policyParam.insured.vehicleCountry = "THA";
            policyParam.insured.vehicleModelDescription = "TOYO20160104";
            policyParam.insured.vehicleRegistrationNo = "CN" + randomStr;
            policyParam.insured.vehicleGarageType = VehicleGarageType.GARAGE;
            policyParam.insured.vehicleMakeName = "TOYOTA";
            policyParam.insured.vehicleProvince = "THA";
            policyParam.insured.vehicleModelName = "COROLLA";
            policyParam.insured.vehicleRegistrationYear = 2016;
            policyParam.insured.vehicleUsage = VehicleUsage.PRIVATE;
            policyParam.insured.vehicleModelYear = 2016;

            policyParam.payer = new Payer();
            // Address
            // Category - InThaiAddress / OutThaiAddress
            policyParam.payer.inThaiAddress = new InThaiAddress();
            policyParam.payer.inThaiAddress.district = "1001";
            policyParam.payer.inThaiAddress.postalCode = "10200";
            policyParam.payer.inThaiAddress.province = "10";
            policyParam.payer.inThaiAddress.street = "songhu rd.";
            policyParam.payer.inThaiAddress.subDistrict = "100101";
            policyParam.payer.name = "leon luo";

            // PolicyHolder 
            // Category - IndiPolicyholder / OrgPolicyHolder
            // For Example: policyParam.orgPolicyhodler = new OrganizationPolicyholder();
            policyParam.indiPolicyholder = new IndividualPolicyholder();
            policyParam.indiPolicyholder.idNo = "123456";
            policyParam.indiPolicyholder.idType = "1";
            policyParam.indiPolicyholder.inThaiAddress = new InThaiAddress();
            policyParam.indiPolicyholder.inThaiAddress.fullAddress = "24 (318 เดิม) ซ.อุดมสุข30 แยก2 ถ.อุดมสุข แขวงบางนา เขตบางนา กทม. 10260";
            policyParam.indiPolicyholder.inThaiAddress.district = "1001";
            policyParam.indiPolicyholder.inThaiAddress.postalCode = "10200";
            policyParam.indiPolicyholder.inThaiAddress.province = "10";
            policyParam.indiPolicyholder.inThaiAddress.street = "songhu rd.";
            policyParam.indiPolicyholder.inThaiAddress.subDistrict = "100101";
            policyParam.indiPolicyholder.lastName = "luo";
            policyParam.indiPolicyholder.firstName = "leon";
            policyParam.indiPolicyholder.mobile = "1234999";
            policyParam.indiPolicyholder.taxNo = "10000";
            policyParam.indiPolicyholder.title = IndividualPrefix.Khun;

            // Driver List
            policyParam.drivers = new List<Driver>(1);
            Driver driver = new Driver();
            policyParam.drivers.Add(driver);
            driver.birthday = DateTime.Now;
            driver.firstName = "leon";
            driver.lastName = "luo";
            driver.occupation = "1233333";

            // Invoke service method to get issue result.
            // Response
            // Success true / false
            // errorMessage
            // policyNo 

            IssuedResp issueResp = service.Issue(resp.token, policyParam);
            if (issueResp.success)
            {
               Console.WriteLine("Issued succcess: true" + "\nPolicyNo:" + issueResp.policyNo);
            }
            else
            {
               Console.WriteLine("Issued succcess: false" + "\nMessage:" + issueResp.message);
            }

			//test file upload
			//UploadFileParams uploadFileParams = new UploadFileParams();
			//uploadFileParams.fileInfo = new System.IO.FileInfo("./Main.cs");
			//uploadFileParams.uploadExtraData = new JObject();
			//uploadFileParams.uploadExtraData["policyId"] = 1;
			//uploadFileParams.uploadExtraData["docName"] = "test";
			//uploadFileParams.uploadExtraData["docType"] = "1";

			//NetworkUtils.UploadFile(ApiConsts.API_DOCS, uploadFileParams,resp.token);

           // Console.WriteLine("{0}", calcResp);
            Console.ReadKey();

        }

    }
}
