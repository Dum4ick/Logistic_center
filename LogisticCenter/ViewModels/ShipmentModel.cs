using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class ShipmentModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }
        [JsonPropertyName("warehouse_name")]
        public string WarehouseName { get; set; }
        [JsonPropertyName("status_id")]
        public string StatusId { get; set; }
        [JsonPropertyName("status_name")]
        public string StatusName { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
    }

}
