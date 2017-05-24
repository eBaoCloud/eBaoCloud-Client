using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ebaocloud.client.thai.seg.cmi.response
{
    public class VehicleModel : CascadeValue
    {
        public String vehicleType { get; set; }
        public Decimal marketPrice { get; set; }
        public String capacity { get; set; }
        public String vehicleGroup { get; set; }
        public String vehicleMarketGroup { get; set; }
        public String numOfSeat { get; set; }
        public String tonnage { get; set; }
    }
}
