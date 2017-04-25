# eBaoCloud-sdk-dotNet
This project is aimed to provide an alternative way to easily
integrate to eBaoCloud Thailand. If you are building your
application on Microsoft DotNet platform
and don't want to consume eBaoCloud Thailand's restful API,
it will be your right choice.

Checkout [Demo project](https://github.com/eBaoCloud/eBaoCloud-sdk-dotNet-demo)

Licensed under the Apache License 2.0.

### Insurers & Products supported

- SEG VMI, CMI, VMI(buy CMI together)

### Installation

To install with **nuget**, run the following command in the *Package Manager Console*:

```
PM> Install-Package com.ebaocloud.client.thai.seg.vmi
PM> Install-Package com.ebaocloud.client.thai.seg.cmi
...
```

### Usage (C#)

- Calculate Premium:


```C#
PolicyService service = new PolicyServiceImpl();
LoginResp resp = service.Login(Login.sampleUserName, Login.samplePassword);

var calculationParams = new CalculationParams();
calculationParams.effectiveDate = DateTime.Now.ToLocalTime();
calculationParams.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
calculationParams.proposalDate = DateTime.Now.ToLocalTime();
calculationParams.productCode = "CMI";
calculationParams.vehicleUsage = VehicleUsage.PRIVATE;

CalculationResp calcResp = service.Calculate(resp.token, calculationParams);
if (calcResp.success)
{
   Console.WriteLine("Calculate succcess: true");
}
else
{
    Console.WriteLine("Calculate succcess: false" + "\nMessage:" + calcResp.errorMessage);
}
```


- Issue Policy:


```
PolicyService service = new PolicyServiceImpl();
LoginResp resp = service.Login(Login.sampleUserName, Login.samplePassword);

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

String randomStr = new Random(DateTime.Now.Millisecond).Next().ToString();
policyParam.insured = new Insured();
policyParam.insured.vehicleChassisNo = "CN" + randomStr;
policyParam.insured.vehicleRegistrationNo = "CN" + randomStr;
policyParam.insured.vehicleType = VehicleType.Sedan;
policyParam.insured.vehicleSubType = VehicleSubType.Car_Seat_up_to_7_people;
policyParam.insured.vehicleColor = "white";
policyParam.insured.vehicleCountry = "THA";
policyParam.insured.vehicleModelDescription = "Sedan 4dr G  6sp FWD 2.5 2016";
policyParam.insured.vehicleMakeName = "TOYOTA";
policyParam.insured.vehicleProvince = "THA";
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

IssuedResp issueResp = service.Issue(resp.token, policyParam);
 if (issueResp.success)
{
    Console.WriteLine("Issued succcess: true" + "\nPolicyNo:" + issueResp.policyNo);
} else
{
    Console.WriteLine("Issued succcess: false" + "\nMessage:" + issueResp.message);
}
```


In the above samples, there is a optional step called "Login", please be note that
before you can make a successful call, Login is the must, 
but it's not necessary to do it every time, once you have called Login API, a token
will be returned, actually your system could keep it somewhere with safety,
could be memory or database, next time, you can using the token to make other API calls.
By doing this, the performance will be much better.

The stored token should be updated with new one regularly, the recommended frequency is one day.

Looking for more samples with details? Please refer to [Demo project](https://github.com/eBaoCloud/eBaoCloud-sdk-dotNet-demo)

### In Other Programming Languages

- Java: in progress
- PHP: in progress
- Ruby: in progress

### Recent Releases

21 Apr 2017, 0.1 released

Feature - add SEG VMI/CMI/VMI(buy CMI together) support




