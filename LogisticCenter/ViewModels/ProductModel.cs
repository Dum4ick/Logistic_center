using System.Text.Json.Serialization;

namespace LogisticCenter
{
    public class ProductModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Name { get; set; }

        public string CategoryID { get; set; }
        public string Category { get; set; }

        public string Weight { get; set; }
        public string Price { get; set; }
    }


}
