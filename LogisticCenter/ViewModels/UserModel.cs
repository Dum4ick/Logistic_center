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
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class DataModel
    {
        [JsonPropertyName("status")]
        public string Response { get; set; }

        [JsonPropertyName("data")]
        public List<UserModel> UsersList { get; set; }
    }
}
