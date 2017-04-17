using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.seg.client.pub;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class CalculationParams : BaseModel
    {
        public String productCode { get; set; }
        public String planCode { get; set; }
        public DateTime proposalDate { get; set; }
        public DateTime effectiveDate { get; set; }
        public DateTime expireDate { get; set; }

        public String vehicleMakeName { get; set; }
        public int vehicleModelYear { get; set; }
        public String vehicleModelDescription { get; set; }
        public int vehicleRegistrationYear { get; set; }
        public Decimal vehicleAccessaryValue { get; set; }
        public Decimal vehicleTotalValue { get; set; }
        public VehicleUsage vehicleUsage { get; set; }
        public VehicleGarageType vehicleGarageType { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
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
