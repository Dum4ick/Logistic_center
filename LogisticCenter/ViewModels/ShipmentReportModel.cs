using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class ShipmentReportModel
    {
        public string Shipment_Id { get; set; }
        public string Order_Id { get; set; }
        public string Status { get; set; }
        public string Created_At { get; set; }
        public string Items_Count { get; set; }
    }

}
