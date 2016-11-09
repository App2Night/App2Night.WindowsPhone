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


namespace App2Night.APIObjects
{
    public class Party
    {
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public DateTime CreationDate { get; set; }
        public int Price { get; set; }
        public User Host { get; set; }
        public DateTime PartyDate { get; set; }
        public MusicGenre MusicGenre { get; set; }
        public PartyType PartyType { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }

    }
}