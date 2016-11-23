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
        public Guid PartyId { get; set; }
        [JsonProperty("PartyName")]
        [MaxLength(32)]
        public string PartyName { get; set; }
        [JsonProperty("CreationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty("Price")]
        public int Price { get; set; }
        [JsonProperty("Host")]
        public User Host { get; set; }
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
        [JsonProperty("HostedByUser")]
        public bool HostedByUser { get; set; }
        public EventCommitmentState CommitmentState { get; set; }
    }
}