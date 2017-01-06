using App2Night.ModelsEnums.Model;
using App2Night.Ressources;
using Plugin.Geolocator;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace App2Night.Logik
{
    public class GeolocationLogik
    {
        /// <summary>
        /// Gibt den aktuellen Standort als Latitude und Longitude zurück.
        /// </summary>
        /// <returns>Aktuellen Standort des Nutzers</returns>
        public async Task<Location> GetLocation()
        {
            // Code von http://stackoverflow.com/questions/32323942/windows-universal-uwp-geolocation-api-permissions

            CancellationTokenSource _cts;
            // Erlaubnis zwischenspeichern
            UserEinstellungen einst = new UserEinstellungen();
            einst = await DatenVerarbeitung.UserEinstellungenAuslesen();

            try
            {
                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                if (accessStatus == GeolocationAccessStatus.Allowed)
                {
                    einst.GPSErlaubt = true;
                    await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

                    // Get cancellation token
                    _cts = new CancellationTokenSource();
                    CancellationToken token = _cts.Token;

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };

                    // Carry out the operation
                    var _pos = await geolocator.GetGeopositionAsync().AsTask(token);

                    return new Location()
                    {
                        Latitude = (float)_pos.Coordinate.Point.Position.Latitude,
                        Longitude = (float)_pos.Coordinate.Point.Position.Longitude
                    };
                }
                else if (accessStatus == GeolocationAccessStatus.Denied)
                {
                    // Wenn der Nutzer der App nicht den Standort zur Verfügung stellt, wird der Nutzer infomiert, dass die App dann nicht komplett verwendet werden kann
                    einst.GPSErlaubt = false;
                    await DatenVerarbeitung.UserEinstellungenSpeichern(einst);
                    var message = new MessageDialog(Meldungen.Hauptansicht.FehlerGPSNoetig, "Achtung!");
                    return null;
                }
                else
                {
                    throw new Exception("Problem with location permissions or access");
                }

            }
            catch (TaskCanceledException tce)
            {
                throw new Exception("Task cancelled" + tce.Message);
            }
            finally
            {
                _cts = null;
            }
        }
    }
}
