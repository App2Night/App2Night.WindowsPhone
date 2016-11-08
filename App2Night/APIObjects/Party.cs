using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace App2Night.APIObjects
{
    public class Party
    {
        public string PartId { get; set; }
        public int Price { get; set; }
        public string PartyName { get; set; }
        public string PartyDate { get; set; }
        public int MusicGenre { get; set; }
        public Location Location { get; set; }
        public int PartyType { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
    }


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

    public class User
     {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public Location Location { get; set; }
    }
}