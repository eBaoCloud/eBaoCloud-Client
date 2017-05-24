using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebaocloud.client.thai.seg.cmi.response;
using Newtonsoft.Json.Linq;
using com.ebaocloud.client.thai.seg.cmi.pub;

namespace com.ebaocloud.client.thai.seg.cmi.api
{
    public class MasterDataServiceImpl : MasterDataService
    {

        public List<CascadeValue> GetVehicleMakes()
        {
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_VEHICLE_MAKE);
            return GetCascateInfo(response);
        }

        public List<CascadeValue> GetVehicleMakeModels(string makeKey)
        {
            if (makeKey == null)
            {
                throw new Exception("Vehicle make key is required.");
            }
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_VEHICLE_MAKE_MODEL + makeKey);
            return GetCascateInfo(response);
        }

        public List<CascadeValue> GetVehicleModelYears(string makeKey, string modelKey)
        {
            if (makeKey == null || modelKey == null)
            {
                throw new Exception("[GetVehicleModelYears] Make and model is required.");
            }
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_VEHICLE_MODEL_YEAR + makeKey + "/" + modelKey);
            return GetCascateInfo(response);

        }

        public List<VehicleModel> GetVehicleModelInfo(string makeKey, string modelKey, string modelYear)
        {
            if (makeKey == null || modelKey == null || modelYear == null)
            {
                throw new Exception("[GetVehicleModelInfo] Make model and modelYear is required.");
            }
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_VEHICLE_MODEL_INFO + makeKey + "/" + modelKey + "/" + modelYear);
            JArray data = (JArray)response["data"];
            List<VehicleModel> result = new List<VehicleModel>();
            foreach (JObject obj in data)
            {
                VehicleModel vehicleModel = new VehicleModel();
                vehicleModel.parentKey = (string)obj["pid"];
                vehicleModel.key = (string)obj["id"];
                vehicleModel.value = (string)obj["text"];
                vehicleModel.capacity = (string)obj["capacity"];
                vehicleModel.marketPrice = (decimal)obj["marketPrice"];
                vehicleModel.numOfSeat = (string)obj["numOfSeat"];
                vehicleModel.tonnage = (string)obj["tonnage"];
                vehicleModel.vehicleGroup = (string)obj["vehicleGroup"];
                vehicleModel.vehicleMarketGroup = (string)obj["vehicleMarketGroup"];
                vehicleModel.vehicleType = (string)obj["vehicleType"];
                result.Add(vehicleModel);
            }
            return result;
        }

        public List<KeyValue> GetVehicleUsage(string vehicleType)
        {
            if (vehicleType == null)
            {
                throw new Exception("[GetVehicleUsage] VehicleType is required.");
            }

            Dictionary<string, string> parameter = GenerateVehicleParams("vmiVehicleUsage");
            List<KeyValue> result = new List<KeyValue>();
            JObject response = NetworkUtils.Post(ApiConsts.API_MISC_MASTER_DATA, parameter);
            JArray data = (JArray)response["data"];
            foreach (JObject obj in data)
            {
                String p = (string)obj["p"];
                if (p.Equals(vehicleType))
                {
                    KeyValue keyValue = new KeyValue();
                    keyValue.key = (string)obj["id"];
                    keyValue.value = (string)obj["text"];
                    result.Add(keyValue);
                }
            }
            return result;
        }

        public List<KeyValue> GetVehicleGarageType()
        {
            return GetMasterDataWithCodeTable("garageType");
        }

        public List<KeyValue> GetVehicleCountry()
        {
            return GetMasterDataWithCodeTable("vehicleProvince");
        }

        public List<KeyValue> GetVehicleColor()
        {
            return GetMasterDataWithCodeTable("color");
        }

        public List<KeyValue> GetBeneficiary()
        {
            return GetMasterDataWithCodeTable("beneficiary");
        }

        public List<KeyValue> GetOccupation()
        {
            return GetMasterDataWithCodeTable("occupation");
        }

        public List<KeyValue> GetPrefix(PrefixType prefixType)
        {
            switch (prefixType)
            {
                case PrefixType.INDIVIDUAL:
                    {
                        return GetMasterDataWithCodeTable("indiPrefix");
                    }
                case PrefixType.ORGANIZATION:
                    {
                        return GetMasterDataWithCodeTable("corpPrefix");
                    }
                default:
                    return GetMasterDataWithCodeTable("indiPrefix");
            }
        }

        public List<KeyValue> GetCustomType()
        {
            return GetMasterDataWithCodeTable("customerType");
        }

        public List<KeyValue> GetCounty()
        {
            return GetMasterDataWithCodeTable("country");
        }

        public List<KeyValue> GetAddressType()
        {
            return GetMasterDataWithCodeTable("thaiAddressType");
        }

        public List<KeyValue> GetIdType()
        {
            return GetMasterDataWithCodeTable("idType");
        }

        public List<CascadeValue> GetAddressProvince()
        {
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_ADDRESS_PROVINCE);
            return GetCascateInfo(response);
        }

        public List<CascadeValue> GetDistrict(string provinceKey)
        {
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_ADDRESS_DISTRICT + provinceKey);
            return GetCascateInfo(response);
        }

        public List<Address> GetSubDistrict(string provinceKey, string districtKey)
        {
            JObject response = NetworkUtils.Get(ApiConsts.API_MISC_ADDRESS_SUB_DISTRICT + provinceKey + "/" + districtKey);
            JArray data = (JArray)response["data"];
            List<Address> result = new List<Address>();
            foreach (JObject obj in data)
            {
                Address cascadeValue = new Address();
                cascadeValue.parentKey = (string)obj["pid"];
                cascadeValue.key = (string)obj["id"];
                cascadeValue.value = (string)obj["text"];
                cascadeValue.postalCode = (string)obj["post"];
                result.Add(cascadeValue);
            }
            return result;
        }

        public List<KeyValue> GetUploadDocumentType()
        {
            return GetMasterDataWithCodeTable("uploadDocType");
        }


        private static Dictionary<string, string> GenerateVehicleParams(string codeTableName)
        {
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            parameter.Add("insurerTenantCode", "SEG_TH");
            parameter.Add("prdtCode", "VMI");
            parameter.Add("prdtVersion", "v1");
            parameter.Add("codeTableName", codeTableName);
            return parameter;
        }


        private static List<CascadeValue> GetCascateInfo(JObject response)
        {
            JArray data = (JArray)response["data"];
            List<CascadeValue> result = new List<CascadeValue>();
            foreach (JObject obj in data)
            {
                CascadeValue cascadeValue = new CascadeValue();
                cascadeValue.parentKey = (string)obj["pid"];
                cascadeValue.key = (string)obj["id"];
                cascadeValue.value = (string)obj["text"];
                result.Add(cascadeValue);
            }
            return result;
        }

        private List<KeyValue> GetMasterDataWithCodeTable(string codeTableName)
        {
            Dictionary<string, string> parameter = GenerateVehicleParams(codeTableName);
            JObject response = NetworkUtils.Post(ApiConsts.API_MISC_MASTER_DATA, parameter);
            JArray data = (JArray)response["data"];
            List<KeyValue> result = new List<KeyValue>();
            foreach (JObject obj in data)
            {
                KeyValue keyValue = new KeyValue();
                keyValue.key = (string)obj["id"];
                keyValue.value = (string)obj["text"];
                result.Add(keyValue);
            }
            return result;
        }
    }
}
