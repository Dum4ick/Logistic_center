using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class OrderItemModel
    {
    [JsonPropertyName("name")]
    public string ProductName { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
     [JsonPropertyName("price")]
    public decimal Price { get; set; }

    public decimal Total => Quantity * Price;
}


}
