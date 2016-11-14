using System.Threading.Tasks;
using App2Night.BackEndCommunication;
using Newtonsoft.Json;
using Windows.Data.Json;
using App2Night.APIObjects;
using System.Collections.Generic;
using System.Linq;

namespace App2Night.Controller
{
    class FensterHauptansichtController
    {
        public static async Task<IEnumerable<Party>> partyListeVonServerGET()
            {
                string dataFromServer;
                dataFromServer = await BackEndComParty.GetRequest();
                IEnumerable<Party> partyListe = JsonConvert.DeserializeObject<IEnumerable<Party>>(dataFromServer);
                return partyListe;
            }

        public static async Task<Party> einePartyVonServer()
        {
            IEnumerable<Party> res = await partyListeVonServerGET();           
            return res.FirstOrDefault();

        }
    }
}
