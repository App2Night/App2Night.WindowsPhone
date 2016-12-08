using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class Location
    {
        [JsonProperty("CountryName")]
        public string CountryName { get; set; }
        [JsonProperty("Cityname")]
        public string CityName { get; set; }
        [JsonProperty("StreetName")]
        public string StreetName { get; set; }
        [JsonProperty("HouseNumber")]
        public string HouseNumber { get; set; }
        [JsonProperty("Zipcode")]
        public string ZipCode { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }
}
