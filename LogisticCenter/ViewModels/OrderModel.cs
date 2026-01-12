using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class OrderModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("warehouse_id")]
        public string WarehouseId { get; set; }
        [JsonPropertyName("warehouse")]
        public string WarehouseName { get; set; }
        [JsonPropertyName("status_id")]
        public string StatusId { get; set; }
        [JsonPropertyName("status")]
        public string StatusName { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
    }


}
