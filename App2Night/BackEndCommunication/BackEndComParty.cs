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
using Windows.UI.Popups;
using App2Night.ModelsEnums.Model;

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

        private static HttpClient GetClientParty()
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
                HttpClient client = GetClientParty();
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
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    message.ShowAsync();
                    // Code 21 - Fehler bei Abrufen
                    return "21";
                }

            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                message.ShowAsync();
                return "42";
            }




        }




        public static async Task<Token> GetToken(string username, string password)
        {
            Token token = new Token();

            try
            {
                using (HttpClient client = GetClientParty())
                {
                    client.BaseAddress = new Uri("http://app2nightuser.azurewebsites.net/");
                    client.DefaultRequestHeaders.Host = "app2nightuser.azurewebsites.net";
                    client.DefaultRequestHeaders.Accept.Clear();
                    var query = "client_id=nativeApp&" +
                                "client_secret=secret&" +
                                "grant_type=password&" +
                                $"username={username}&" +
                                $"password={password}&" +
                                "scope=App2NightAPI offline_access&" +
                                "offline_access=true";
                    var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var requestResult = await client.PostAsync("connect/token", content);

                    if (requestResult.IsSuccessStatusCode)
                    {
                        string response = await requestResult.Content.ReadAsStringAsync();
                        token = JsonConvert.DeserializeObject<Token>(response);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return token;
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
