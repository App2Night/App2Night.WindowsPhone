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
    class BackEndCom
    {
        HttpClient client = new HttpClient();

        private HttpClient GetClient()
        { 
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://app2nightapi.azurewebsites.net/api/") }; 
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
            client.DefaultRequestHeaders.Host = "app2nightapi.azurewebsites.net"; 
            return client; 
        }


        public async Task<String> getRequest()
        {
            string stringFromServer = "";
            bool internetVorhanden = IsInternet();

            if (internetVorhanden == true)
            {
                HttpClient client = GetClient();

                //Send the GET request asynchronously and retrieve the response as a string.
                HttpResponseMessage httpResponse = new HttpResponseMessage();

                try
                {
                    // GET Request
                    httpResponse = await client.GetAsync("https://app2nightapi.azurewebsites.net/api/Party"
                        );
                    httpResponse.EnsureSuccessStatusCode();
                    stringFromServer = await httpResponse.Content.ReadAsStringAsync();
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
        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
    }
    

}
