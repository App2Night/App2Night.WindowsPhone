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
using App2Night.ModelsEnums.Model;
using App2Night.ModelsEnums.Enums;


namespace App2Night.ModelsEnums.Model
{
    public class Party
    {
        [JsonProperty("PartyId")]
        public string PartyId { get; set; }
        [JsonProperty("PartyName")]
        [MaxLength(32)]
        public string PartyName { get; set; }
        [JsonProperty("CreationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty("Price")]
        public double Price { get; set; }
        //[JsonProperty("Host")]
        //public User Host { get; set; }
        [JsonProperty("HostedByUser")]
        public bool HostedByUser { get; set; }
        [JsonProperty("HostId")]
        public int HostId { get; set; }
        [JsonProperty("PartyDate")]
        public DateTime PartyDate { get; set; }
        [JsonProperty("MusicGenre")]
        public MusicGenre MusicGenre { get; set; }
        [JsonProperty("PartyType")]
        public PartyType PartyType { get; set; }
        [JsonProperty("Description")]
        [MaxLength(256)]
        public string Description { get; set; }
        [JsonProperty("Location")]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "UserCommitmentState")]
        public EventCommitmentState UserCommitmentState { get; set;}

        //[JsonProperty("GeneralVoting")]
        //public PartyVoting Voting { get; set; }

        // Voting
        [JsonProperty("GeneralUpVoting")]
        public int GeneralUpVoting { get; set; }
        [JsonProperty("GeneralDownVoting")]
        public int GeneralDownVoting { get; set; }
        //[JsonProperty("PriceUpVoting")]
        //public int PriceUpVoting { get; set; }
        //[JsonProperty("PriceDownVoting")]
        //public int PriceDownVoting { get; set; }
        //[JsonProperty("LocationUpVoting")]
        //public int LocationUpVoting { get; set; }
        //[JsonProperty("LocationDownVoting")]
        //public int LocationDownVoting { get; set; }
        //[JsonProperty("MoodUpVoting")]
        //public int MoodUpVoting { get; set; }
        //[JsonProperty("MoodDownVoting")]
        //public int MoodDownVoting { get; set; }


    }
}