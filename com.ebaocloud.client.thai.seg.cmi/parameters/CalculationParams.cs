using System;
using com.ebaocloud.client.thai.seg.cmi.pub;
using System.Reflection;

namespace com.ebaocloud.client.thai.seg.cmi.parameters
{
	public class CalculationParams : BaseModel
	{
		public String productCode { get; set; }
		public DateTime proposalDate { get; set; }
        public DateTime effectiveDate { get; set; }
        public DateTime expireDate { get; set; }

        public String vehicleUsage { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] infoList = type.GetProperties();
            foreach (PropertyInfo item in infoList)
            {
                string name = item.Name;
                Object value = item.GetValue(this, null);
                 if (typeof(DateTime).IsInstanceOfType(value))
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

}
