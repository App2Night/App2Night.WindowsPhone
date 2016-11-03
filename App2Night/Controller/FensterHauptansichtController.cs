using System.Threading.Tasks;
using App2Night.BackEndCommunication;
using Newtonsoft.Json;
using Windows.Data.Json;
using App2Night.APIObjects;

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

        public static async Task<Party.RootObject> DatenFromServerToParty()
        {
            string data;
            JsonArray partyArray;
            //var party;
            string[] partyFertig = new string[15];

            //gibt x Partys aus
            data = await DataFromServerGET();
            // Partys in JsonArray 
            partyArray = JsonArray.Parse(data);
            // Array in Object, vom Object die Eigenschaft in einen String;
            //party = partyArray[0];

            Party.RootObject partyEins = new Party.RootObject();
            partyEins = Party.FromStringToParty(data);

            


            return partyEins;

        }



    }



}
