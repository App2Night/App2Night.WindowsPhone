using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Windows.Networking.Connectivity;
using Windows.Data.Json;

//https://github.com/App2Night/App2Night.Xamarin/blob/dev/PartyUp.Service/Service/ClientService.cs
//http://app2nightapi.azurewebsites.net/swagger/ui/index.html

namespace App2Night.BackEndCommunication
{
    public class BackEndComParty
    {
        public BackEndComParty()
        {

        }

        HttpClient client = new HttpClient();

        private static HttpClient GetClient()
        { 
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://app2nightapi.azurewebsites.net/api/") }; 
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
            client.DefaultRequestHeaders.Host = "app2nightapi.azurewebsites.net"; 
            return client; 
        }


        public static async Task<string> GetRequest()
        {
            string stringFromServer = "";
            bool internetVorhanden = IsInternet();

            if (internetVorhanden == true)
            {
                HttpClient client = GetClient();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();

                try
                {
                    // GET Request
                    httpAntwort = await client.GetAsync("http://app2nightapi.azurewebsites.net/api/Party");
                   
                    httpAntwort.EnsureSuccessStatusCode();
                    stringFromServer = await httpAntwort.Content.ReadAsStringAsync();
                    return stringFromServer;
                }
                catch (Exception ex)
                {
                    stringFromServer = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    // Code 21 - Fehler bei Abrufen
                    return "21";
                }

            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                return "42";
            }



            
        }




        /// <summary>
        /// Checken, ob Intenet eingeschaltet ist.
        /// </summary>
        /// <returns>Boolwert über Internetstatus</returns>
        private static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
    }
    

}
