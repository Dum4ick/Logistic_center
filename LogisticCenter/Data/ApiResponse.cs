using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.Data
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("user")]
        public T User { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }


}
