using App2Night.ModelsEnums.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ModelsEnums.Model
{
    public class PartyVoting
    {
        // Zahlen = Bedeutung
        // 0 = nicht bewertet
        // 1 = positiv bewertet
        // -1 = negativ bewertet

        private int priceRatingVar;
        [JsonProperty("priceRating")]
        public int priceRating
        {
            get { return priceRatingVar; }
            set { priceRatingVar = 0; }
        }

        private int moodRatingVar;
        [JsonProperty("moodRating")]
        public int moodRating
        {
            get { return moodRatingVar; }
            set { moodRatingVar = 0; }
        }

        private int locationRatingVar;
        [JsonProperty("locationRating")]
        public int locationRating
        {
            get { return locationRatingVar; }
            set { locationRatingVar = 0; }


        }
        private int generalRatingVar;
        [JsonProperty("generalRating")]
        public int generalRating
        {
            get { return generalRatingVar; }
            set { generalRatingVar = 0; }


        }
    }
}

