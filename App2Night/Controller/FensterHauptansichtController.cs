using System.Threading.Tasks;
using App2Night.BackEndCommunication;
using Newtonsoft.Json;
using Windows.Data.Json;
using System.Collections.Generic;
using System.Linq;
using App2Night.ModelsEnums.Model;

namespace App2Night.Controller
{
    class FensterHauptansichtController
    {
        public static async Task<IEnumerable<Party>> partyListeVonServerGET()
            {
                string dataFromServer;
                dataFromServer = await BackEndComParty.GetParties();
                IEnumerable<Party> partyListe = JsonConvert.DeserializeObject<IEnumerable<Party>>(dataFromServer);
                return partyListe;
            }
    }
}
