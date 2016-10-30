using System.Threading.Tasks;
using App2Night.BackEndCommunication;
using Newtonsoft.Json;
using Windows.Data.Json;

namespace App2Night.Controller
{
    class FensterHauptansichtController
    {
        public static async Task<string> DataFromServerGET()
            {
                string dataFromServer;
                dataFromServer = await BackEndComParty.GetRequest();
                return dataFromServer;
            }

        public static async Task<string> DatenFromServerToArray()
        {
            string data;
            JsonArray partyArray;
            //JsonObject eineParty;
            
            //gibt x Partys aus
            data = DataFromServerGET().Result;
            // Partys in JsonArray 
            partyArray = JsonConvert.DeserializeObject<JsonArray>(data);
            
            // Array in Object, vom Object die Eigenschaft in einen String
            //eineParty = partyArray.GetObjectAt(0);

            int anzahlPartys = partyArray.Count;

            for (int i = 0; i < anzahlPartys; i++)
            {
                   
            }



            return data;

        }



    }



}
