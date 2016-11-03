using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

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


            // ID aus Party
            partyArray = data.Split(',');
            string partyIdAusString = partyArray[0];
            // Das letzte Zeichen '\' abschneiden
            partyIdAusString = partyIdAusString.Remove((partyIdAusString.Length - 1));
            // Die ersten Zeichen abschneiden, nur für ID
            //TODO: Nach Änderungen in API eventuell anpassen!
            partyIdAusString = partyIdAusString.Substring(12);

            if (partyIdAusString.Length < 36 || partyIdAusString.Length > 36)
            {
                //Fehler beim "Zuschneiden"
            }

            partyAusString.partId = partyIdAusString;


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

            partyAusString.partyName = partyNameAusString;

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

            partyAusString.partyDate = datumAusString;



            return partyAusString;
        }

        public static string[] VonPartyRootObjectToArray(Party.RootObject partyRootObject)
        {
            string[] partyDaten = new string[20];

            if (partyRootObject.partId != null)
            {
                partyDaten[0] = partyRootObject.partId;
            }

            if (partyRootObject.price != -1)
            {
                partyDaten[1] = partyRootObject.price.ToString();
            }

            if (partyRootObject.partyName != null)
            {
                partyDaten[2] = partyRootObject.partyName;
            }

            if (partyRootObject.partyDate != null)
            {
                partyDaten[3] = partyRootObject.partyDate;
            }

            if (partyRootObject.musicGenre != -1)
            {
                partyDaten[4] = partyRootObject.musicGenre.ToString();
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

            if (partyRootObject.partyType != -1)
            {
                partyDaten[13] = partyRootObject.partyType.ToString(); 
            }

            if (partyRootObject.description != null)
            {
                partyDaten[14] = partyRootObject.description; 
            }

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

 
        public class Host
        {
            public string userId { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public object location { get; set; }
        }

        public class Location
        {
            public string countryName { get; set; }
            public string cityName { get; set; }
            public string streetName { get; set; }
            public int houseNumber { get; set; }
            public string houseNumberAdditional { get; set; }
            public int zipcode { get; set; }
            public int latitude { get; set; }
            public int longitude { get; set; }
        }

        public class RootObject
        {
            public string partId { get; set; }
            public string date { get; set; }
            public int price { get; set; }
            public Host host { get; set; }
            public string partyName { get; set; }
            public string partyDate { get; set; }
            public int musicGenre { get; set; }
            public Location location { get; set; }
            public int partyType { get; set; }
            public string description { get; set; }
        }








    }
}
