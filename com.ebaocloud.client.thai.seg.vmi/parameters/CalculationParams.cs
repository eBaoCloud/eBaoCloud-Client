using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebaocloud.client.thai.seg.vmi.pub;
using System.Reflection;

namespace com.ebaocloud.client.thai.seg.vmi.parameters
{
    public class CalculationParams : BaseModel
    {
        public String productCode { get; set; }
        public String planCode { get; set; }
        public DateTime proposalDate { get; set; }
        public DateTime effectiveDate { get; set; }
        public DateTime expireDate { get; set; }
        public String vehicleMakeName { get; set; }
        public String vehicleModelName { get; set; }
        public int vehicleModelYear { get; set; }
        public String vehicleModelDescription { get; set; }
        public int vehicleRegistrationYear { get; set; }
        public String vehicleUsage { get; set; }
        public String vehicleGarageType { get; set; }

        public Decimal vehicleAccessaryValue { get; set; }
        public Decimal vehicleTotalValue { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

        public void validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propList = type.GetProperties();
            foreach (PropertyInfo item in propList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (name != "vehicleAccessaryValue" && name != "vehicleTotalValue")
                {
                    if (typeof(int).IsInstanceOfType(value) || typeof(VehicleGarageType).IsInstanceOfType(value) || typeof(VehicleUsage).IsInstanceOfType(value))
                    {
                        if ((int)value == 0)
                        {
                            throw new Exception(name + " is required");
                        }
                    } else
                    {
                        if (value == null)
                        {
                            throw new Exception(name + " is required");
                        }
                    }
                }
            }
        }
    }

    public enum VehicleUsage
    {
        PRIVATE = 110,
        COMERCIAL = 120
    }

    public enum VehicleGarageType
    {
        GARAGE = 1,
        DEALER
    }

    
}
