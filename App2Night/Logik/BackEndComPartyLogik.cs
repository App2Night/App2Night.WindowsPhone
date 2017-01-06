using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using App2Night.ModelsEnums.Model;

//https://github.com/App2Night/App2Night.Xamarin/blob/dev/PartyUp.Service/Service/ClientService.cs
//http://app2nightapi.azurewebsites.net/swagger/ui/index.html

namespace App2Night.Logik
{
    public class BackEndComPartyLogik
    {
        public BackEndComPartyLogik()
        {

        }

        HttpClient client = new HttpClient();

        /// <summary>
        /// Client für die Backend-Kommunikation mit app2nightapi.azurewebsites.net
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
        public static async Task<IEnumerable<Party>> GetParties(Location aktuellePosition, float radius)
        {
            string stringFromServer = "";
            bool internetVorhanden = IsInternet();
            IEnumerable<Party> partyListe = null;

            bool erfolg = await DatenVerarbeitung.aktuellerToken();
            Token tok = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && erfolg == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                double latitude = aktuellePosition.Latitude;
                double longitude = aktuellePosition.Longitude;

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tok.AccessToken);

                string anfrage = $"Party?lat={latitude}&lon={longitude}&radius={radius}";
                // durch das Speichern in den String wird aus einem Punkt ein Komma, deshalb muss das danach ausgebessert werden
                anfrage = anfrage.Replace(',', '.'); 

                try
                {
                    httpAntwort = await client.GetAsync(anfrage);                   
                    stringFromServer = await httpAntwort.Content.ReadAsStringAsync();
                    partyListe = JsonConvert.DeserializeObject<IEnumerable<Party>>(stringFromServer);
                }
                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
            }

            return partyListe;
        }

        /// <summary>
        /// Gibt die Daten einer bestimmten Party (ID) zurueck.
        /// </summary>
        /// <param name="id">ID der Party</param>
        /// <returns>Party</returns>
        public static async Task<Party> GetPartyByID(string id)
        {
            string stringFromServer = "";
            bool internetVorhanden = IsInternet();
            Party party = new Party();

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();

                try
                {
                    httpAntwort = await client.GetAsync($"Party/id={id}");
                    stringFromServer = await httpAntwort.Content.ReadAsStringAsync();
                    party = JsonConvert.DeserializeObject<Party>(stringFromServer);
                }
                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
            }

            return party;
        }

        /// <summary>
        /// Erstellt eine Party mit allen wichtigen Informationen und schickt sie zur Sicherung an den Server.
        /// </summary>
        /// <param name="party">Zu erstellende Party</param>
        /// <returns>Erfolg</returns>
        public static async Task<bool> CreateParty(Party party)
        {
            bool internetVorhanden = IsInternet();
            // Umspeichern der Daten für BackEnd
            CreateParty partyZuErstellen = new CreateParty();
            partyZuErstellen.PartyName = party.PartyName;
            partyZuErstellen.PartyDate = party.PartyDate;
            partyZuErstellen.MusicGenre = party.MusicGenre;
            partyZuErstellen.CountryName = "Deutschland";
            partyZuErstellen.CityName = party.Location.CityName;
            partyZuErstellen.StreetName = party.Location.StreetName;
            partyZuErstellen.HouseNumber = party.Location.HouseNumber;
            partyZuErstellen.Zipcode = party.Location.ZipCode;
            partyZuErstellen.PartyType = party.PartyType;
            partyZuErstellen.Description = party.Description;
            partyZuErstellen.Price = party.Price;

            bool erfolg = await DatenVerarbeitung.aktuellerToken();
            Token tok = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && erfolg == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tok.AccessToken);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(partyZuErstellen), Encoding.UTF8, "application/json");              

                try
                {
                    httpAntwort = await client.PostAsync("Party", content);
                    bool erfolgreich = httpAntwort.IsSuccessStatusCode;
                    return erfolgreich;
                }

                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    return false;
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
                return false;
            }
        }
    
        /// <summary>
        /// Loescht eine bestimmte Party (ID) vom Server.
        /// </summary>
        /// <param name="id">Zu loeschende Party</param>
        /// <returns>Erfolg</returns>
        public static async Task<bool> DeletePartyByID(Party party)
        {
            bool internetVorhanden = IsInternet();
            bool status = false;

            bool erfolg = await DatenVerarbeitung.aktuellerToken();
            Token token = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && erfolg == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

                try
                {
                    httpAntwort = await client.DeleteAsync($"Party/?id={party.PartyId}");
                    status = httpAntwort.IsSuccessStatusCode;
                    return status;
                }

                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    return false;
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
                return false;
            }
        }

        /// <summary>
        /// Schickt die neuen Informationen einer bereits erstellten Party an den Server.
        /// </summary>
        /// <param name="id">ID der zu aktualisierenden Party</param>
        /// <param name="party">Party mit neuen Werten</param>
        /// <returns>Erfolg</returns>
        public static async Task<bool> UpdatePartyByID(Party party)
        {
            bool internetVorhanden = IsInternet();
            bool erfolg = false;
            // Umspeichern der Daten für BackEnd
            CreateParty partyZuErstellen = new CreateParty();
            partyZuErstellen.PartyName = party.PartyName;
            partyZuErstellen.PartyDate = party.PartyDate;
            partyZuErstellen.MusicGenre = party.MusicGenre;
            partyZuErstellen.CountryName = "Deutschland";
            partyZuErstellen.CityName = party.Location.CityName;
            partyZuErstellen.StreetName = party.Location.StreetName;
            partyZuErstellen.HouseNumber = party.Location.HouseNumber;
            partyZuErstellen.Zipcode = party.Location.ZipCode;
            partyZuErstellen.PartyType = party.PartyType;
            partyZuErstellen.Description = party.Description;
            partyZuErstellen.Price = party.Price;

            bool aktuellerToken = await DatenVerarbeitung.aktuellerToken();
            Token token = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && aktuellerToken == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(partyZuErstellen), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PutAsync($"Party/?id={party.PartyId}", content);
                    erfolg = httpAntwort.IsSuccessStatusCode;
                    return erfolg;
                }

                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    return false;
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
                return false;
            }
        }

        /// <summary>
        /// Validiert die eingegebene Adresse und gibt die Adresse laut Google zurueck.
        /// </summary>
        /// <param name="location">Zu pruefende Adresse</param>
        /// <returns>Location laut Google</returns>
        public static async Task<string> ValidateLocation(Location location)
        {
            bool internetVorhanden = IsInternet();
            string adresseLautGoogle = "";
            location.CountryName = "Deutschland";

            bool aktuellerToken = await DatenVerarbeitung.aktuellerToken();
            Token token = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && aktuellerToken == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                var jsonLocation = JsonConvert.SerializeObject(location);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
                HttpContent content = new StringContent(jsonLocation, Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PostAsync("Party/validate", content);
                    adresseLautGoogle = await httpAntwort.Content.ReadAsStringAsync();
                    return adresseLautGoogle;
                }

                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    return "21";
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
                return "42";
            }
        }

        /// <summary>
        /// Post des Votings.
        /// </summary>
        /// <param name="party">Party, bei der gevotet wird.</param>
        /// <param name="voting">Voting</param>
        /// <returns></returns>
        public static async Task<bool> PutPartyRating(Party party, PartyVoting voting)
        {
            bool internetVorhanden = IsInternet();
            bool erfolgreichesVoting = false;

            bool erfolg = await DatenVerarbeitung.aktuellerToken();
            Token tok = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && erfolg == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tok.AccessToken);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(voting), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PutAsync($"UserParty/partyRating?id={party.PartyId}", content);
                    erfolgreichesVoting = httpAntwort.IsSuccessStatusCode;
                    return erfolgreichesVoting;
                }

                catch (Exception)
                {
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    return false;
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
                return false;
            }
        }

        /// <summary>
        /// Post des Teilnahme-Status.
        /// </summary>
        /// <param name="party">Party, bei der sich der Teilnahme-Status ändert.</param>
        /// <param name="teilnahme">Teilnahmestatus</param>
        /// <returns></returns>
        public static async Task<bool> PutPartyCommitmentState(Party party, CommitmentParty teilnahme)
        {
            bool internetVorhanden = IsInternet();
            bool teilnehmen = false;
            string nachricht = "";

            Login login = await DatenVerarbeitung.LoginAuslesen();
            Token tok = await BackEndComUserLogik.GetToken(login);

            if (internetVorhanden == true)// && erfolg == true)
            {
                HttpClient client = GetClientParty();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                var json = JsonConvert.SerializeObject(teilnahme);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tok.AccessToken);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PutAsync($"UserParty/commitmentState?id={party.PartyId}", content);
                    teilnehmen = httpAntwort.IsSuccessStatusCode;
                    return teilnehmen;
                }

                catch (Exception ex)
                {
                    nachricht = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    // Code 21 - Fehler bei Abrufen
                    return false;
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
                return false;
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
