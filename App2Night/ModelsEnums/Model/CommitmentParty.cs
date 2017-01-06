using App2Night.ModelsEnums.Enums;
using Newtonsoft.Json;

namespace App2Night.ModelsEnums.Model
{
    /// <summary>
    /// Model für Teilnahme-Status
    /// </summary>
    public class CommitmentParty
    {
        [JsonProperty("eventCommitment")]
        public EventCommitmentState Teilnahme { get; set; }
       
    }
}