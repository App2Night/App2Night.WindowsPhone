using App2Night.ModelsEnums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class CreatePartyModel
    {
        public string PartyName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Description { get; set; }
        public DateTime PartyDate { get; set; }
        public PartyType PartyType { get; set; }
        public MusicGenre MusicGenre { get; set; }
        public double Price { get; set; }

    }
}
