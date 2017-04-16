using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Http;
using Newtonsoft.Json.Linq;
using com.ebao.gs.ebaocloud.sea.seg.client;
using System.Dynamic;
using com.ebao.gs.ebaocloud.sea.seg.client.pub;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.api;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.response;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;

namespace com.ebao.gs.ebaocloud.sea.seg.client
{
    public class TestNetwork
    {
        public static void Main(string[] args)
        {
            PolicyService service = new PolicyServiceImplement();
            LoginResp resp = service.Login("1110124560", "eBao1234");
            ////Resp json = JsonConvert.DeserializeObject<Resp>(response.RawText);         
            Console.WriteLine("{0}", resp);

            var calculationParams = new CalculationParams();
            calculationParams.effectiveDate = DateTime.Now.ToLocalTime();
            calculationParams.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
            calculationParams.proposalDate = DateTime.Now.ToLocalTime();

            calculationParams.planCode = "TIB";
            calculationParams.productCode = "VMI";
            calculationParams.productVersion = "v1";
           // calculationParams.vehicleAccessaryValue = 1000;
            calculationParams.vehicleCapacity = 0;
           // calculationParams.vehicleClass = "";
            calculationParams.vehicleCode = "110";
            calculationParams.vehicleGarageType = "Garage";
            calculationParams.vehicleGroup = 3;
            calculationParams.vehicleMakeCode = "TOYOTA";
            calculationParams.vehicleMarketValue = 1569000;
            calculationParams.vehicleModelCode = "CAMRY";
           // calculationParams.vehicleModelDescription = "";
            calculationParams.vehicleModelYear = "2016";
            calculationParams.vehicleNumOfSeats = 5;
            calculationParams.vehicleRegistrationYear = 2016;
            calculationParams.vehicleTonnage = 1;
            //calculationParams.vehicleTotalValue = 200000;


            CalculationResp calcResp = service.Calculate(resp.token, calculationParams);
            Console.WriteLine("{0}", calcResp);
            Console.ReadKey();
        }

    }
}
