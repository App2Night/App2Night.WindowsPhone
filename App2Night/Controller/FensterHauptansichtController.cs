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

            //Geopoint po = new Geopoint(baspo);
            Geopoint geopoint = GetGeoLocation.GetLocation().Result;
            float radius = 30;

            dataFromServer = await BackEndComParty.GetParties(geopoint, radius);
            IEnumerable<Party> partyListe = JsonConvert.DeserializeObject<IEnumerable<Party>>(dataFromServer);
            return partyListe;
        }
    }
}
