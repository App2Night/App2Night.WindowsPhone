using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Gibt ID, Name und Datum mit Uhrzeit der Party für die Anzeige aus
        /// //TODO: weitere Umwandlung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RootObject FromStringToParty(string data)
        {
            string[] partyArray;
            string[] partyFertig = new string[20];
            RootObject partyAusString = new RootObject { };
            //TODO: Nach Änderungen in API eventuell anpassen!

            // ID aus Party
            partyArray = data.Split(',');
            string partyIdAusString = partyArray[0];
            // Das letzte Zeichen '\' abschneiden
            partyIdAusString = partyIdAusString.Remove((partyIdAusString.Length - 1));
            // Die ersten Zeichen abschneiden, nur für ID
            partyIdAusString = partyIdAusString.Substring(12);

            if (partyIdAusString.Length < 36 || partyIdAusString.Length > 36)
            {
                //Fehler beim "Zuschneiden"
            }

            partyAusString.PartId = partyIdAusString;


            // Name aus Party
            string partyNameAusString = partyArray[2];
            // Das letzte Zeichen '\' abschneiden
            partyNameAusString = partyNameAusString.TrimEnd('\\', '"');
            // Die ersten Zeichen abschneiden, nur für ID
            partyNameAusString = partyNameAusString.Substring(13);

            if (partyNameAusString.Contains("PartyName"))
            {
                //Fehler beim "Zuschneiden"
            }

            partyAusString.PartyName = partyNameAusString;

            // Datum aus Party
            string datumAusString = partyArray[3];
            // Das letzte Zeichen '\' abschneiden
            datumAusString = datumAusString.TrimEnd('\\', '"');
            // Die ersten Zeichen abschneiden, nur für ID
            datumAusString = datumAusString.Substring(13);

            //if (datumAusString.Contains())
            //{
            //    //Fehler beim "Zuschneiden"
            //}

            partyAusString.PartyDate = datumAusString;



            return partyAusString;
        }

        public static string[] VonPartyRootObjectToArray(Party.RootObject partyRootObject)
        {
            string[] partyDaten = new string[20];

            if (partyRootObject.PartId != null)
            {
                partyDaten[0] = partyRootObject.PartId;
            }

            if (partyRootObject.Price != -1)
            {
                partyDaten[1] = partyRootObject.Price.ToString();
            }

            if (partyRootObject.PartyName != null)
            {
                partyDaten[2] = partyRootObject.PartyName;
            }

            if (partyRootObject.PartyDate != null)
            {
                partyDaten[3] = partyRootObject.PartyDate;
            }

            if (partyRootObject.MusicGenre != -1)
            {
                partyDaten[4] = partyRootObject.MusicGenre.ToString();
            }

            //if (partyRootObject.location.countryName != null)
            //{
            //    partyDaten[5] = partyRootObject.location.countryName;
            //}

            //if (partyRootObject.location.cityName != null)
            //{
            //    partyDaten[6] = partyRootObject.location.cityName;
            //}

            //if (partyRootObject.location.streetName != null)
            //{
            //    partyDaten[7] = partyRootObject.location.streetName;
            //}

            //if (partyRootObject.location.houseNumber != -1)
            //{
            //    partyDaten[8] = partyRootObject.location.houseNumber.ToString();
            //}

            //if (partyRootObject.location.houseNumberAdditional != null)
            //{
            //    partyDaten[9] = partyRootObject.location.houseNumberAdditional;
            //}

            //if (partyRootObject.location.zipcode != -1)
            //{
            //    partyDaten[10] = partyRootObject.location.zipcode.ToString(); 
            //}

            //if (partyRootObject.location.latitude != 0)
            //{
            //    partyDaten[11] = partyRootObject.location.latitude.ToString(); 
            //}

            //if (partyRootObject.location.longitude != 0)
            //{
            //    partyDaten[12] = partyRootObject.location.longitude.ToString(); 
            //}

            //if (partyRootObject.partyType != -1)
            //{
            //    partyDaten[13] = partyRootObject.partyType.ToString(); 
            //}

            //if (partyRootObject.description != null)
            //{
            //    partyDaten[14] = partyRootObject.description; 
            //}

            //if (partyRootObject.host.userId != null)
            //{
            //    partyDaten[15] = partyRootObject.host.userId; 
            //}

            //if (partyRootObject.host.username != null)
            //{
            //    partyDaten[16] = partyRootObject.host.username; 
            //}

            return partyDaten;
        }

        [DataContract]
        public class Location
        {
            [DataMember]
            public string CountryName { get; set; }
            [DataMember]
            public string CityName { get; set; }
            [DataMember]
            public string StreetName { get; set; }
            [DataMember]
            public int HouseNumber { get; set; }
            [DataMember]
            public string HouseNumberAdditional { get; set; }
            [DataMember]
            public int Zipcode { get; set; }
            [DataMember]
            public int Latitude { get; set; }
            [DataMember]
            public int Longitude { get; set; }
        }

        [DataContract]
        public class Host
        {
            [DataMember]
            public string HostId { get; set; }
            [DataMember]
            public string UserName { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public string PartId { get; set; }
            [DataMember]
            public int Price { get; set; }
            [DataMember]
            public string PartyName { get; set; }
            [DataMember]
            public string PartyDate { get; set; }
            [DataMember]
            public int MusicGenre { get; set; }
            [DataMember]
            public Location Location { get; set; }
            [DataMember]
            public int PartyType { get; set; }
            [DataMember]
            public string Description { get; set; }
            [DataMember]
            public Host Host { get; set; }

            //public RootObject(string stringFromServer)
            //{
            //    JArray array = JArray.Parse(stringFromServer);

            //    JObject jObject = (JObject)array[0];

            //    if (stringFromServer != "" || stringFromServer != " ")
            //    {
            //        try
            //        {
            //            jObject = JObject.Parse(stringFromServer);
            //        }
            //        catch (Exception)
            //        {

            //            //throw;
            //        }

            //        JArray obj = JArray.Parse(stringFromServer);

            //        JToken jParty = jObject["party"];

            //        PartId = (string)jParty["PartId"];
            //        Price = (int)jParty["Price"];
            //        PartyName = (string)jParty["PartyName"];
            //        MusicGenre = (int)jParty["MusicGenre"];
            //        Location.CountryName = (string)jParty["CountryName"];
            //        Location.CityName = (string)jParty["CountryName"];
            //        Location.StreetName = (string)jParty["StreetName"];
            //        Location.HouseNumber = (int)jParty["HouseNumber"];
            //        Location.HouseNumberAdditional = (string)jParty["HouseNumberAdditional"];
            //        Location.Zipcode = (int)jParty["Zipcode"];
            //        Location.Latitude = (int)jParty["Latitude"];
            //        Location.Longitude = (int)jParty["Longitude"];
            //        PartyType = (int)jParty["PartyType"];
            //        Description = (string)jParty["Description"];
            //        Host.HostId = (string)jParty["HostId"];
            //        Host.UserName = (string)jParty["UserName"];

            //    }
            //}

            //public RootObject()
            //{

            //}
        }


    }
}