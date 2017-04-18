using System;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub;
using System.Reflection;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters
{
	public class CalculationParams : BaseModel
	{
		public String productCode { get; set; }
		public DateTime proposalDate { get; set; }
        public DateTime effectiveDate { get; set; }
        public DateTime expireDate { get; set; }

        public VehicleUsage vehicleUsage { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] infoList = type.GetProperties();
            foreach (PropertyInfo item in infoList)
            {
                string name = item.Name;
                Object value = item.GetValue(this, null);
                if (typeof(VehicleUsage).IsInstanceOfType(value))
                {
                    if ((int)value == 0)
                    {
                        throw new Exception(String.Format("In {0} {1} is required.", type.Name, name));
                    }
                } else if (typeof(DateTime).IsInstanceOfType(value))
                {
                    DateTime date = (DateTime)value;
                    
                         
                } else
                {
                    if (value == null)
                    {
                        throw new Exception(String.Format("In {0} {1} is required.", type.Name, name));
                    }
                }
            }
        }
    }

	public enum VehicleUsage
	{
		PRIVATE = 1,
		RENT,
		PUBLIC_RENT
	}
}
