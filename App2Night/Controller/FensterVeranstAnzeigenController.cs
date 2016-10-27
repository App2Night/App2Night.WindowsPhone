using App2Night.BackEndCommunication;
using App2Night.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App2Night.APIObjects.Party;

namespace App2Night.Controller
{
    public class FensterVeranstAnzeigenController
    {
        public FensterVeranstAnzeigenController()
        {

        }

        public static async Task DataFromServerZumAnzeigenKompletteParty()
        {
            //var taskWartenAufDaten = BackEndComParty.getRequest();
            //taskWartenAufDaten.Wait(); // Blocks current thread until GetFooAsync task completes
            // For pedagogical use only: in general, don't do this!
            // Das krieg ich vom Server
            //var dataFromServer = taskWartenAufDaten.Result.ToString(); ;



            string dataFromServer = await BackEndComParty.GetRequest();
            // Umwandeln 
            var jsonObject = JsonConvert.DeserializeObject<RootObject>(dataFromServer);

                              

        }


    }
}
