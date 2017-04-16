using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class CalculationParams
    {
        //String tenantCode;
        //String bizType;
        public String productCode;
        public String productVersion;
        public String planCode;

        //String policySource;
        public DateTime proposalDate;
        public DateTime effectiveDate;
        public DateTime expireDate;

        public String vehicleCode;
        public String vehicleClass;
        public String vehicleMakeCode;
        public String vehicleModelCode;
        public String vehicleModelYear;
        public String vehicleModelDescription;
        public int vehicleRegistrationYear;
        public String vehicleGarageType;
        public int vehicleGroup;
        public int vehicleCapacity;
        public int vehicleTonnage;
        public int vehicleNumOfSeats;
        public Decimal vehicleMarketValue;
        public Decimal vehicleAccessaryValue;
        public Decimal vehicleTotalValue;
    }
}
