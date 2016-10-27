﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.APIObjects
{
    public class Party
    {
        public class Host
        {
            public string userId { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public object location { get; set; }
        }

        public class Location
        {
            public string countryName { get; set; }
            public string cityName { get; set; }
            public string streetName { get; set; }
            public int houseNumber { get; set; }
            public string houseNumberAdditional { get; set; }
            public int zipcode { get; set; }
            public int latitude { get; set; }
            public int longitude { get; set; }
        }

        public class RootObject
        {
            public string partId { get; set; }
            public string date { get; set; }
            public int price { get; set; }
            public Host host { get; set; }
            public string partyName { get; set; }
            public string partyDate { get; set; }
            public int musicGenre { get; set; }
            public Location location { get; set; }
            public int partyType { get; set; }
            public string description { get; set; }
        }
    }
}
