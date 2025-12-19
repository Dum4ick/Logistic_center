using System.Text.Json.Serialization;

namespace LogisticCenter
{
    public class UserModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonPropertyName("role_id")]
        public string RoleId { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
    }
}
