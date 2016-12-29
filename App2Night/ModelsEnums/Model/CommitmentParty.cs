using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2Night.ModelsEnums.Model;
using App2Night.ModelsEnums.Enums;
using Newtonsoft.Json;

namespace App2Night.ModelsEnums.Model
{
    public class CommitmentParty
    {
        [JsonProperty("eventCommitment")]
        public EventCommitmentState Teilnahme { get; set; }
       
    }
}