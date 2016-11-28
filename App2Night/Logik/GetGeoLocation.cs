﻿using App2Night.ModelsEnums.Model;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace App2Night.Logik
{
    public class GeolocationLogik
    {
        /// <summary>
        /// Gibt den aktuellen Standort als Latitude und Longitude oder CivicAddress zurueck.
        /// </summary>
        /// <returns>Aktuellen Standort des Nutzers</returns>
        public async Task<Location> GetLocation()
        {
            CancellationTokenSource _cts;
            try
            {

                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                if (accessStatus == GeolocationAccessStatus.Allowed)
                {
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
                else
                    throw new Exception("Problem with location permissions or access");

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
