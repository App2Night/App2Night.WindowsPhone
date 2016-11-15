using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class Login
    {
        [JsonProperty("Username")]
        public string Username { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
    }
}
