using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class Location
    {
        public int LocationId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string HouseNumberAdditional { get; set; }
        public string Zipcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
