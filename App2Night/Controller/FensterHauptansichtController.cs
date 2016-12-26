using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Data.Json;
using System.Collections.Generic;
using System.Linq;
using App2Night.ModelsEnums.Model;
using Windows.Devices.Geolocation;
using Plugin.Geolocator.Abstractions;
using App2Night.Logik;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using System;
using App2Night.Views;

namespace App2Night.Controller
{
    public class FensterHauptansichtController 
    {
        FensterHauptansicht fha = new FensterHauptansicht();

        public FensterHauptansichtController()
        {

        }

        public static async Task<IEnumerable<Party>> btnInDerNaehePartysAbrufen()
        {
            IEnumerable<Party> partyListe = null;
            Location pos;

            var geoLocation = new GeolocationLogik();

            pos = await geoLocation.GetLocation();
            
            // Radius aus UserEinstellungen
            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();
            float radius = einst.Radius;

            partyListe = await BackEndComPartyLogik.GetParties(pos, radius);

            return partyListe;

        }
    }
}


