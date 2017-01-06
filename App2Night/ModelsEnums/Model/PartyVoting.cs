using Newtonsoft.Json;

namespace App2Night.ModelsEnums.Model
{
    /// <summary>
    ///  Model fürs Voting einer Party.
    /// </summary>
    public class PartyVoting
    {
        // Zahlen = Bedeutung
        // 0 = nicht bewertet
        // 1 = positiv bewertet
        // -1 = negativ bewertet

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

