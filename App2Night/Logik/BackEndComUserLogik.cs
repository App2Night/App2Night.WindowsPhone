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

namespace App2Night.Logik
{
    public class BackEndComUserLogik
    {
        public BackEndComUserLogik()
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

        public static async Task<bool> GetUserInfo()
        {
            string stringFromServer = "";
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();
            Login login = await DatenVerarbeitung.DatenAusDateiLesenLogin();
            // aktueller Token wird benötigt
            bool erfolg = await DatenVerarbeitung.aktuellerToken();
            Token tok = await DatenVerarbeitung.DatenAusDateiLesenToken();

            if (internetVorhanden == true && erfolg == true)
            {
                HttpClient client = GetClientUser();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tok.AccessToken);

                try
                {
                    // GET Request
                    httpAntwort = await client.GetAsync("connect/userinfo");
                    //httpAntwort.EnsureSuccessStatusCode();
                    login.userID = await httpAntwort.Content.ReadAsStringAsync();
                    // ID in die Datei schreiben
                    await DatenVerarbeitung.DatenInDateiSchreibenLogin(login);
                    return true;
                }
                catch (Exception ex)
                {
                    stringFromServer = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
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
        /// Erstellt einen neuen User und gibt die dazugehörige Guid aus.
        /// </summary>
        /// <param name="login">Benutzername, Passwort und Emailadresse fuer neuen User.</param>
        /// <returns>Guid</returns>
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
                    httpAntwort = await client.PostAsync("User", content);
                    bool erfolgreich = httpAntwort.IsSuccessStatusCode;
                    return erfolgreich;
                }

                catch (Exception ex)
                {
                    var fehler = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
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

        public static async Task<bool> ResetPasswort(Login login)
        {
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();
            string emailadresse = login.Email;

            if (internetVorhanden == true)
            {
                HttpClient client = GetClientUser();
                HttpResponseMessage httpAntwort = new HttpResponseMessage();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(emailadresse), Encoding.UTF8, "application/json");

                try
                {
                    httpAntwort = await client.PostAsync("User/ResetPassword", content);
                    bool erfolgreich = httpAntwort.IsSuccessStatusCode;
                    return erfolgreich;
                }

                catch (Exception ex)
                {
                    var fehler = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
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
        /// Loescht den durch die Guid angegebenen User.
        /// </summary>
        /// <param name="userID">Guid des Users.</param>
        /// <returns>Status</returns>
        // Token fehlt und momentan ist im Server noch ein Problem -> geht nicht
        public static async Task<string> DeleteUser(string userID)
        {
            string status = "";
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();

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
                        await message.ShowAsync();
                    }

                    return status;
                }

                catch (Exception ex)
                {
                    status = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                    // Code 21 - Fehler bei Abrufen
                    return "21";
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
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
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();

            if (internetVorhanden == true)
            {
                try
                {
                    HttpClient client = GetClientUser();

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
                catch (Exception ex)
                {
                    string error = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                } 
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
            }
            return token;
        }

        /// <summary>
        /// Erneuert den Token.
        /// </summary>
        /// <returns>Token mit weiteren Daten</returns>
        public static async Task<bool> RefreshToken(Token token)
        {
            bool internetVorhanden = BackEndComPartyLogik.IsInternet();
            bool erfolg = false;

            if (internetVorhanden == true)
            {
                try
                {
                    using (HttpClient client = GetClientUser())
                    {
                        client.BaseAddress = new Uri("http://app2nightuser.azurewebsites.net/");
                        //client.DefaultRequestHeaders.Host = "http://app2nightuser.azurewebsites.net";
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);
                        //client.DefaultRequestHeaders.Accept.Clear();
                        var query =     "client_id=nativeApp&" +
                                              "client_secret=secret&" +
                                              $"token={token.RefreshToken}&" +
                                              "token_type_hint=refresh_token";
                        var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
                        
                        var requestResult = await client.PostAsync("connect/revocation", content);

                        if (requestResult.IsSuccessStatusCode)
                        {
                            erfolg = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    var message = new MessageDialog("Fehler! Bitte versuche es später erneut.");
                    await message.ShowAsync();
                }
            }
            else
            {
                // Nachricht, dass Internet eingeschaltet werden soll
                // Code 42 - Fehler: Keine Internetverbindung
                var message = new MessageDialog("Fehler! Keiner Internetverbindung.");
                await message.ShowAsync();
            }
            return erfolg;
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
