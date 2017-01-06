using App2Night.ModelsEnums.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class CreateParty
    {
        [JsonProperty("PartyName")]
        public string PartyName { get; set; }
        [JsonProperty("PartyDate")]
        public DateTime PartyDate { get; set; }
        [JsonProperty("MusicGenre")]
        public MusicGenre MusicGenre { get; set; }
        [JsonProperty("CountryName")]
        public string CountryName { get; set; }
        [JsonProperty("CityName")]
        public string CityName { get; set; }
        [JsonProperty("StreetName")]
        public string StreetName { get; set; }
        [JsonProperty("HouseNumber")]
        public string HouseNumber { get; set; }
        [JsonProperty("Zipcode")]
        public string Zipcode { get; set; }
        [JsonProperty("PartyType")]
        public PartyType PartyType { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Price")]
        public int Price { get; set; }
    }
}
