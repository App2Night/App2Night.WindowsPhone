using Newtonsoft.Json;

namespace App2Night.ModelsEnums.Model
{
    /// <summary>
    /// Model für die Login-Daten
    /// </summary>
    public class Login
    {
        [JsonProperty("sub")]
        public string userID { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
