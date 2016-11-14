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

        private static HttpClient GetClientUser()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://app2nightuser.azurewebsites.net") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Host = "app2nightuser.azurewebsites.net";
            return client;
        }

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
                    httpAntwort = await client.PostAsync("/api/User", content);
                    // 400 existiert bereits
                    // 201 erfolg
                    httpAntwort.EnsureSuccessStatusCode();
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
                string url = $"/api/User/id={userID}";

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

        public static string stringBereinigen(string userID)
        {
            string bereinigteUserID = "";

            bereinigteUserID = userID.Remove(userID.Length - 1, 1);
            bereinigteUserID = bereinigteUserID.Remove(0, 1);

            return bereinigteUserID;
        }
    }
}
