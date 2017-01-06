using App2Night.ModelsEnums.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace App2Night.Logik
{
    public class BackEndComUserLogik
    {
        HttpClient client = new HttpClient();

        public BackEndComUserLogik()
        {

        }

        /// <summary>
        /// Client fuer die Backend-Kommunikation mit app2nightuser.azurewebsites.net
        /// </summary>
        /// <returns>Client</returns>
        private static HttpClient GetClientUser()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://app2nightuser.azurewebsites.net/") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = "app2nightuser.azurewebsites.net";
            return client;
        }

        /// <summary>
        /// UserID wird geholt und in File für Login geschrieben.
        /// </summary>
        /// <returns>Erfolg</returns>
        public static async Task<bool> GetUserInfo()
        {
            string stringFromServer = "";
            Login id;
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();
            Login login = await DatenVerarbeitung.LoginAuslesen();

            // aktueller Token wird benötigt
            bool erfolg = await DatenVerarbeitung.aktuellerToken();
            Token tok = await DatenVerarbeitung.TokenAuslesen();

            if (internetVorhanden == true && erfolg == true)
            {
                HttpClient client = GetClientUser();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tok.AccessToken);

                try
                {
                    httpAntwort = await client.GetAsync("connect/userinfo");
                    stringFromServer = await httpAntwort.Content.ReadAsStringAsync();
                    id = JsonConvert.DeserializeObject<Login>(stringFromServer);
                    login.userID = id.userID;

                    // ID in die Datei schreiben
                    await DatenVerarbeitung.LoginSpeichern(login);
                    return true;
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
        /// Erstellt einen neuen User und gibt die dazugehörige Guid aus.
        /// </summary>
        /// <param name="login">Login fuer neuen User.</param>
        /// <returns>Erfolg</returns>
        public static async Task<bool> CreateUser(Login login)
        {
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();
            login.userID = "0";

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientUser();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PostAsync("api/User", content);
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
        /// Fordert Token für Nutzer an, um dann Partys zu erstellen/aendern/loeschen.
        /// </summary>
        /// <param name="login">Benoetigt Nutzername und Passwort des Users.</param>
        /// <returns>Token mit weiteren Daten</returns>
        public static async Task<Token> GetToken(Login login)
        {
            Token token = new Token();
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();

            if (internetVorhanden == true)
            {
                try
                {
                    HttpClient client = GetClientUser();
                    var query =     "client_id=nativeApp&" +
                                            "client_secret=secret&" +
                                            "grant_type=password&" +
                                            $"username={login.Username}&" +
                                            $"password={login.Password}&" +
                                            "scope=App2NightAPI offline_access openid";

                        var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
                        var requestResult = await client.PostAsync("connect/token", content);

                        if (requestResult.IsSuccessStatusCode)
                        {
                            string response = await requestResult.Content.ReadAsStringAsync();
                            token = JsonConvert.DeserializeObject<Token>(response);
                        }
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
            return token;
        }

        /// <summary>
        /// Erneuert den Token und speichert den neuen in die Datei.
        /// </summary>
        /// <returnsErfolg</returns>
        public static async Task<bool> RefreshToken(Token token)
        {
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();
            bool erfolg = false;

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientUser(); 
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

                var query =     "client_id=nativeApp&" +
                                "client_secret=secret&" +
                                $"token={token.RefreshToken}&" +
                                 "token_type_hint=access_token";
                var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");

                var httpAntwort = await client.PostAsync("connect/revocation", content);

                erfolg = httpAntwort.IsSuccessStatusCode;
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
            }
            return erfolg;
        }
    
    }
}
