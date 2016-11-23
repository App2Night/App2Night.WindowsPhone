using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace App2Night.Controller
{
    public class GetGeoLocation
    {
        /// <summary>
        /// Gibt den aktuellen Standort als Latitude und Longitude oder CivicAddress zurueck.
        /// </summary>
        /// <returns>Aktuellen Standort des Nutzers</returns>
        public static async Task<Geoposition> GetLocation()
        {
            // TODO: Geoposition scheinbar veraltet
            Geolocator geolocator = new Geolocator();
            Geoposition pos = null;

            var accessStatus = await Geolocator.RequestAccessAsync();

            if (GeolocationAccessStatus.Allowed == accessStatus)
            {
                pos = await geolocator.GetGeopositionAsync();
            }
            else
            {
                var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                message.ShowAsync();
            }

            return pos;
        }
    }
}
