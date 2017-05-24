using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebaocloud.client.thai.seg.cmi.response;

namespace com.ebaocloud.client.thai.seg.cmi.api
{
    public interface MasterDataService
    {
        List<CascadeValue> GetVehicleMakes();
        List<CascadeValue> GetVehicleMakeModels(string makeKey);
        List<CascadeValue> GetVehicleModelYears(string makeKey, string modelKey);
        List<VehicleModel> GetVehicleModelInfo(string makeKey, string modelKey, string modelYear);

        List<KeyValue> GetVehicleUsage(string vehicleType);
        List<KeyValue> GetVehicleGarageType();
        List<KeyValue> GetVehicleCountry();
        List<KeyValue> GetVehicleColor();
        List<KeyValue> GetBeneficiary();
        List<KeyValue> GetOccupation();
        List<KeyValue> GetPrefix(PrefixType prefixType);
        List<KeyValue> GetCustomType();
        List<KeyValue> GetCounty();
        List<KeyValue> GetAddressType();
        List<KeyValue> GetIdType();

        List<CascadeValue> GetAddressProvince();
        List<CascadeValue> GetDistrict(string provinceKey);
        List<Address> GetSubDistrict(string provinceKey, string districtKey);

        List<KeyValue> GetUploadDocumentType();
    }
    public enum PrefixType
    {
        INDIVIDUAL = 1,
        ORGANIZATION = 2
    }
}
