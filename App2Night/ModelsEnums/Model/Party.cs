using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using App2Night.ModelsEnums.Enums;


namespace App2Night.ModelsEnums.Model
{
    /// <summary>
    /// Model für eine Party.
    /// </summary>
    public class Party
    {
        [JsonProperty("PartyId")]
        public Guid PartyId { get; set; }
        [JsonProperty("PartyName")]
        public string PartyName { get; set; }
        [JsonProperty("CreationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty("Price")]
        public int Price { get; set; }
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

        [JsonProperty("UserCommitmentState")]
        public EventCommitmentState UserCommitmentState { get; set;}

        // Voting
        [JsonProperty("GeneralRating")]
        public int GeneralRating { get; set; }
        [JsonProperty("PriceRating")]
        public int PriceRating { get; set; }
        [JsonProperty("LocationRating")]
        public int LocationRating { get; set; }
        [JsonProperty("MoodRating")]
        public int MoodRating { get; set; }


    }
}