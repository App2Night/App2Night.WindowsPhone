using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static async Task<Plugin.Geolocator.Abstractions.Position> GetLocation()
        {
            // TODO: Eigener Task oder Thread?

            Plugin.Geolocator.Abstractions.Position pos = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                pos = position;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }

            //Geolocator geolocator = new Geolocator();
            ////Geoposition pos = null;
            //Plugin.Geolocator.Abstractions.Position pos = null;
            //var locator = CrossGeolocator.Current;
            //locator.DesiredAccuracy = 50;

            //var accessStatus = await Geolocator.RequestAccessAsync();
            ////var accessStatus = GeolocationAccessStatus.Allowed;

            ////TimeSpan timeOutMax = new TimeSpan(0, 0, 20);
            //TimeSpan timeout = new TimeSpan(0, 0, 10);

            //try
            //{
            //    if (GeolocationAccessStatus.Allowed == accessStatus)
            //    {
            //        //pos = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(30), timeout);
            //        pos = await locator.GetPositionAsync(1000);
            //    }
            //    else
            //    {
            //        var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
            //        message.ShowAsync();
            //    }
            //}
            //catch(Exception ex)

            //{
            //    Debug.WriteLine(ex);
            //}

            ////BasicGeoposition basePosition = new BasicGeoposition() { Latitude = pos.Coordinate.Point.Position.Latitude, Longitude = pos.Coordinate.Point.Position.Longitude };
            ////Geopoint point = new Geopoint(basePosition);

            return pos;
        }
    }
}
