using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class RoleModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }
    }

}
