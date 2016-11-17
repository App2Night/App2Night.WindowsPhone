using App2Night.ModelsEnums.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace App2Night.BackEndCommunication
{
    public class BackEndComUser
    {
        public BackEndComUser()
        {

        }

        HttpClient client = new HttpClient();

        /// <summary>
        /// Client fuer die Backend-Kommunikation mit app2nightuser.azurewebsites.net
        /// </summary>
        /// <returns>Client</returns>
        private static HttpClient GetClientUser()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://app2nightuser.azurewebsites.net/api/") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = "app2nightuser.azurewebsites.net";
            return client;
        }

        /// <summary>
        /// Erstellt einen neuen User und gibt die dazugehörige Guid aus.
        /// </summary>
        /// <param name="login">Benutzername, Passwort und Emailadresse fuer neuen User.</param>
        /// <returns>Guid</returns>
        public static async Task<string> CreateUser(Login login)
        {
            string userID = "";
            string iD = "";
            bool internetVorhanden = BackEndComParty.IsInternet();

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientUser();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PostAsync("User", content);
                    // 400 existiert bereits
                    // 201 erfolg
                    //httpAntwort.EnsureSuccessStatusCode();
                    userID = await httpAntwort.Content.ReadAsStringAsync();

                    if (userID.Length > 4) // Muss eine Guid sein
                    {
                        var message = new MessageDialog("Benutzer angelegt!");
                        message.ShowAsync();
                        iD = stringBereinigen(userID);
                    }

                    return iD;
                }

                catch (Exception ex)
                {
                    userID = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
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
        /// Loescht den durch die Guid angegebenen User.
        /// </summary>
        /// <param name="userID">Guid des Users.</param>
        /// <returns>Status</returns>
        // Token fehlt und momentan ist im Server noch ein Problem -> geht nicht
        public static async Task<string> DeleteUser(string userID)
        {
            string status = "";
            bool internetVorhanden = BackEndComParty.IsInternet();

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientUser();
                //HttpResponseMessage httpAntwort = new HttpResponseMessage();
                //HttpContent content = new StringContent(userID);
                string url = $"User/id={userID}";

                try
                {
                    var httpAntwort = await client.DeleteAsync(url);

                    // 200 Erfolg
                    // 404 not found
                    httpAntwort.EnsureSuccessStatusCode();
                    status = await httpAntwort.Content.ReadAsStringAsync();

                    if (status == "200")
                    {
                        var message = new MessageDialog("Benutzer gelöscht!");
                        message.ShowAsync();
                    }

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
        /// Fordert Token fur User an, um dann Partys zu erstellen/aendern oder loeschen.
        /// </summary>
        /// <param name="login">Benoetigt Nutzername und Passwort des Users.</param>
        /// <returns>Token mit weiteren Daten</returns>
        public static async Task<Token> GetToken(Login login)
        {
            Token token = new Token();
            bool internetVorhanden = BackEndComParty.IsInternet();

            if (internetVorhanden == true)
            {
                try
                {
                    using (HttpClient client = GetClientUser())
                    {
                        client.BaseAddress = new Uri("http://app2nightuser.azurewebsites.net/");
                        client.DefaultRequestHeaders.Host = "app2nightuser.azurewebsites.net";
                        //client.DefaultRequestHeaders.Accept.Clear();
                        var query =     "client_id=nativeApp&" +
                                              "client_secret=secret&" +
                                              "grant_type=password&" +
                                              $"username={login.Username}&" +
                                              $"password={login.Password}&" +
                                              "scope=App2NightAPI offline_access&" +
                                              "openid email";
                        var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
                        var requestResult = await client.PostAsync("connect/token", content);

                        if (requestResult.IsSuccessStatusCode)
                        {
                            string response = await requestResult.Content.ReadAsStringAsync();
                            token = JsonConvert.DeserializeObject<Token>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    message.ShowAsync();
                } 
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                message.ShowAsync();
            }
            return token;
        }

        /// <summary>
        /// Erneuert den Token.
        /// </summary>
        /// <returns>Token mit weiteren Daten</returns>
        public static async Task<Token> RefreshToken(Token token)
        {
            bool internetVorhanden = BackEndComParty.IsInternet();

            if (internetVorhanden == true)
            {
                try
                {
                    using (HttpClient client = GetClientUser())
                    {
                        client.BaseAddress = new Uri("http://app2nightuser.azurewebsites.net/");
                        client.DefaultRequestHeaders.Host = "app2nightuser.azurewebsites.net";
                        client.DefaultRequestHeaders.Add("Authorization", "Baerer " + token.AccessToken);
                        //client.DefaultRequestHeaders.Accept.Clear();
                        var query =     "client_id=nativeApp&" +
                                              "client_secret=secret" +
                                              $"token={token.RefreshToken}&" +
                                              "token_type_hint=refresh_token";
                        var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
                        var requestResult = await client.PostAsync("connect/token", content);

                        if (requestResult.IsSuccessStatusCode)
                        {
                            string response = await requestResult.Content.ReadAsStringAsync();
                            token = JsonConvert.DeserializeObject<Token>(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    message.ShowAsync();
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                message.ShowAsync();
            }
            return token;
        }

        /// <summary>
        /// Bereinigt String zu validem Guid.
        /// </summary>
        /// <param name="userID">Guid mit zusaetzlichen Zeichen.</param>
        /// <returns>Validen Guid</returns>
        private static string stringBereinigen(string userID)
        {
            string bereinigteUserID = "";

            bereinigteUserID = userID.Remove(userID.Length - 1, 1);
            bereinigteUserID = bereinigteUserID.Remove(0, 1);

            return bereinigteUserID;
        }
    }
}
