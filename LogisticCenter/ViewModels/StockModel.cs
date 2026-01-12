using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    using System.Text.Json.Serialization;

    public class StockModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("warehouse_id")]
        public string WarehouseId { get; set; }

        [JsonPropertyName("warehouse_name")]
        public string WarehouseName { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }



}
