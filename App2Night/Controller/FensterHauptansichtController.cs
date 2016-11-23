using System.Threading.Tasks;
using App2Night.BackEndCommunication;
using Newtonsoft.Json;
using Windows.Data.Json;
using System.Collections.Generic;
using System.Linq;
using App2Night.ModelsEnums.Model;
using Windows.Devices.Geolocation;

namespace App2Night.Controller
{
    class FensterHauptansichtController
    {
        public static async Task<IEnumerable<Party>> partyListeVonServerGET()
        {
            string dataFromServer;

            BasicGeoposition baspo = new BasicGeoposition() { Latitude = 48.443167, Longitude = 8.694906 };
            //Geopoint po = new Geopoint(baspo);
            Geoposition po = GetGeoLocation.GetLocation().Result;
            Geopoint geopoint = new Geopoint(new BasicGeoposition() { Latitude = po.Coordinate.Latitude, Longitude = po.Coordinate.Longitude });
            float radius = 30;

            dataFromServer = await BackEndComParty.GetParties(po, radius);
            IEnumerable<Party> partyListe = JsonConvert.DeserializeObject<IEnumerable<Party>>(dataFromServer);
            return partyListe;
        }
    }
}
