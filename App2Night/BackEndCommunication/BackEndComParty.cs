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

        /// <summary>
        /// Client fuer die Backend-Kommunikation mit app2nightapi.azurewebsites.net
        /// </summary>
        /// <returns>Client</returns>
        private static HttpClient GetClientParty()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://app2nightapi.azurewebsites.net/api/") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = "app2nightapi.azurewebsites.net";
            return client;
        }

        /// <summary>
        /// Gibt Partys zurueck.
        /// </summary>
        /// <returns>Partys</returns>
        //TODO: GPS und Radius mitschicken
        public static async Task<string> GetParties()
        {
            string stringFromServer = "";
            bool internetVorhanden = IsInternet();

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();

                try
                {
                    // TODO: Geht das noch ohne rest der url?
                    httpAntwort = await client.GetAsync("Party");
                    //httpAntwort.EnsureSuccessStatusCode();
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

        /// <summary>
        /// Gibt die Daten einer bestimmten Party (ID) zurueck.
        /// </summary>
        /// <param name="id">ID der Daten</param>
        /// <returns>Party als String</returns>
        public static async Task<string> GetPartyByID(string id)
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
                    httpAntwort = await client.GetAsync($"Party/id={id}");
                    //httpAntwort.EnsureSuccessStatusCode();
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

        /// <summary>
        /// Erstellt eine Party mit allen wichtigen Informationen und schickt sie zur Sicherung an den Server.
        /// </summary>
        /// <param name="party">Zu erstellende Party mit allen Informationen</param>
        /// <returns>ID</returns>
        public static async Task<string> CreateParty(Party party)
        {
            bool internetVorhanden = IsInternet();
            string status = "";

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(party), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PostAsync("Party", content);
                    //httpAntwort.EnsureSuccessStatusCode();
                    status = await httpAntwort.Content.ReadAsStringAsync();
                    return status;
                }

                catch (Exception ex)
                {
                    status = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
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
    

        /// <summary>
        /// Loescht eine bestimmte Party (ID) vom Server.
        /// </summary>
        /// <param name="id">Zu loeschende Party</param>
        /// <returns>Status</returns>
        public static async Task<string> DeletePartyByID(string id)
        {
            // TODO: Implementieren
            throw new NotImplementedException();
        }

        /// <summary>
        /// Schickt die neuen Informationen einer bereits erstellten Party an den Server.
        /// </summary>
        /// <param name="id">ID der zu aktualisierenden Party</param>
        /// <param name="party">Party mit neuen Werten</param>
        /// <returns>Status</returns>
        public static async Task<string> UpdatePartyByID(string id, Party party)
        {
            // TODO: Implementieren
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validiert die eingegebene Adresse und gibt die Adresse laut Google zurueck.
        /// </summary>
        /// <param name="location">Zu pruefende Adresse</param>
        /// <returns>Location laut Google</returns>
        public static async Task<string> ValidateLocation(Location location)
        {
            bool internetVorhanden = IsInternet();
            // TODO: eventuell Locations statt string
            string adresseLautGoogle = "";

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(location), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PostAsync("/api/Party/validate", content);
                    //httpAntwort.EnsureSuccessStatusCode();
                    // 200 Erfolg
                    adresseLautGoogle = await httpAntwort.Content.ReadAsStringAsync();
                    return adresseLautGoogle;
                }

                catch (Exception ex)
                {
                    adresseLautGoogle = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
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
