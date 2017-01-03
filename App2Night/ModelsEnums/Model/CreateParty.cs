using App2Night.ModelsEnums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class CreateParty
    {
        public string partyName { get; set; }
        public string partyDate { get; set; }
        public MusicGenre musicGenre { get; set; }
        public string countryName { get; set; }
        public string cityName { get; set; }
        public string streetName { get; set; }
        public string houseNumber { get; set; }
        public string zipcode { get; set; }
        public PartyType partyType { get; set; }
        public string description { get; set; }
        public double price { get; set; }
    }
}
