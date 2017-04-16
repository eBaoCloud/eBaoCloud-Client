using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class CalculationParams
    {
        public String productCode;
        public String planCode;
        public DateTime proposalDate;
        public DateTime effectiveDate;
        public DateTime expireDate;

        public String vehicleMakeName;
        public int vehicleModelYear;
        public String vehicleModelDescription;
        public int vehicleRegistrationYear;
        public Decimal vehicleAccessaryValue;
        public Decimal vehicleTotalValue;
        public VehicleUsage vehicleUsage;
        public VehicleGarageType vehicleGarageType;
    }

    public enum VehicleUsage
    {
        PRIVATE = 110,
        COMERCIAL = 120
    }

    public enum VehicleGarageType
    {
        GARAGE,
        DEALER
    }

    
}
