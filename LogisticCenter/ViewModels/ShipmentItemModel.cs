using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{

    public class ShipmentItemModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("shipment_id")]
        public string ShipmentId { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }
    }


}
