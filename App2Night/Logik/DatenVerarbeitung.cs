
using App2Night.ModelsEnums.Model;
using App2Night.Ressources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace App2Night.Logik
{
    /// <summary>
    ///  Die Datenverarbeitung bietet Methoden zum Speichern und Auslesen vom Login, vomm Token und von den Einstellungen des Nutzers.
    /// </summary>
    class DatenVerarbeitung
    {
        private static StorageFolder speicherOrdner = ApplicationData.Current.LocalFolder;
        private static StorageFile speicherDatei;
        private static string DateiLogin = "Login.txt";
        private static string DateiToken = "Token.txt";
        private static string DateiUserEinstellungen = "UserEinst.txt";
        private static string DateiPartys = "Partys.txt";

        /// <summary>
        /// Speichert den übergebenen Login in die dafür vorgesehene Datei. Falls die Datei schon bestehen sollte, wird diese Überschrieben.
        /// </summary>
        /// <param name="neuerNutzer">Login zu speichern</param>
        /// <returns></returns>
        public static async Task<bool> LoginSpeichern(Login neuerNutzer)
        {
            bool erfolg = false;
            // Zieldatei erstellen/überschreiben
            speicherDatei = await speicherOrdner.CreateFileAsync(DateiLogin, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                string loginJsonAlsString = JsonConvert.SerializeObject(neuerNutzer);
                // In Datei schreiben
                await FileIO.WriteTextAsync(speicherDatei, loginJsonAlsString);

                erfolg = true;
            }

            catch (Exception)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }
            return erfolg;
        }

        /// <summary>
        /// Liest die Daten aus der Datei für den Login.
        /// </summary>
        /// <returns></returns>
        public static async Task<Login> LoginAuslesen()
        {
            Login ausDatei = new Login();
            speicherDatei = await speicherOrdner.GetFileAsync(DateiLogin);

            // Daten auslesen
            string text = await FileIO.ReadTextAsync(speicherDatei);

            // Bei Fehler in Datei oder beim Auslesen wird null zurückgegeben.
            if (text != null)
            {
                ausDatei = JsonConvert.DeserializeObject<Login>(text);
            }
            else
            {
                ausDatei = null;
            }

            return ausDatei;
        }

        /// <summary>
        /// Speichert den übergebenen Token in die dafür vorgesehene Datei. Falls die Datei schon bestehen sollte, wird diese Überschrieben.
        /// </summary>
        /// <param name="tok">Token zu speichern</param>
        /// <returns></returns>
        public static async Task<bool> TokenSpeichern(Token tok)
        {
            bool erfolg = false;
            // Zieldatei erstellen/überschreiben
            speicherDatei = await speicherOrdner.CreateFileAsync(DateiToken, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                string tokenJsonAlsString = JsonConvert.SerializeObject(tok);
                // In Datei schreiben
                await FileIO.WriteTextAsync(speicherDatei, tokenJsonAlsString);

                erfolg = true;
            }

            catch (Exception)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }
            return erfolg;
        }

        /// <summary>
        /// Liest die Daten aus der Datei für den Login.
        /// </summary>
        /// <returns></returns>
        public static async Task<Token> TokenAuslesen()
        {
            Token ausDatei = new Token();
            speicherDatei = await speicherOrdner.GetFileAsync(DateiToken);

            // Daten auslesen
            string text = await FileIO.ReadTextAsync(speicherDatei);

            // Bei Fehler in Datei oder beim Auslesen wird null zurückgegeben.
            if (text != null)
            {
                ausDatei = JsonConvert.DeserializeObject<Token>(text);
            }
            else
            {
                ausDatei = null;
            }

            return ausDatei;
        }

        /// <summary>
        /// Überprüft den übergebenen Login auf Korrektheit um zu erfahren, ob der Nutzer existiert.
        /// </summary>
        /// <param name="loginZuPruefen"></param>
        /// <returns></returns>
        public static async Task<bool> LoginUeberpruefen(Login loginZuPruefen)
        {
            bool korrekterLogin = false;

            // Prüfen, ob ein Token erhalten werden kann
            Token tokenLoginUeberpruefung = await BackEndComUserLogik.GetToken(loginZuPruefen);

            // Falls ja, existiert der Nutzer und der Token kann gespeichert werden.
            if (tokenLoginUeberpruefung.AccessToken != null)
            {
                // Speichern des Tokens in Datei
                bool tokenIstGespeichert = await TokenSpeichern(tokenLoginUeberpruefung);

                if (tokenIstGespeichert == true)
                {
                    korrekterLogin = true;
                }
            }

            return korrekterLogin;
        }

        /// <summary>
        /// Schreibt einen aktuellen Token in die dafür vorgesehen Datei. Gibt den Erfolg der Aktion zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> aktuellerToken()
        {
            bool erfolg = false;
            Login login = await DatenVerarbeitung.LoginAuslesen();

            // Der Token wird aus der Datei ausgelesen
            Token aktuellerToken = await TokenAuslesen();

            // Falls in dieser Datei kein Token ist, wird ein neuer Token geholt
            if (aktuellerToken.AccessToken == null)
            {
                aktuellerToken = await BackEndComUserLogik.GetToken(login);

                if (aktuellerToken.AccessToken != null)
                {
                    erfolg = await TokenSpeichern(aktuellerToken);
                }
                else
                {
                    erfolg = false;
                }
            }
            else
            {
                erfolg = true;
            }

            if (erfolg == true)
            {
                // Der Token wird refresht, dass er verwendet werden kann
                erfolg = await BackEndComUserLogik.RefreshToken(aktuellerToken);

                if (erfolg == true)
                {
                    erfolg = await DatenVerarbeitung.TokenSpeichern(aktuellerToken);
                } 
            }

            return erfolg;
        }

        /// <summary>
        /// Speichert die übergebenen Einstellungen in die dafür vorgesehene Datei. Falls die Datei schon bestehen sollte, wird diese Überschrieben.
        /// </summary>
        /// <param name="einst">Einstellungen zu speichern</param>
        /// <returns></returns>
        public static async Task<bool> UserEinstellungenSpeichern(UserEinstellungen einst)
        {
            bool erfolg = false;
            // Zieldatei erstellen/überschreiben
            speicherDatei = await speicherOrdner.CreateFileAsync(DateiUserEinstellungen, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                string userEinstellungenAlsString = JsonConvert.SerializeObject(einst);
                // In Datei schreiben
                await FileIO.WriteTextAsync(speicherDatei, userEinstellungenAlsString);

                erfolg = true;
            }
            catch (Exception)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }
            return erfolg;
        }

        /// <summary>
        /// Liest die Daten aus der Datei für die Nutzereinstellungen.
        /// </summary>
        /// <returns></returns>
        public static async Task<UserEinstellungen> UserEinstellungenAuslesen()
        {
            UserEinstellungen ausDatei = new UserEinstellungen();
            speicherDatei = await speicherOrdner.GetFileAsync(DateiUserEinstellungen);

            // Daten auslesen
            string text = await FileIO.ReadTextAsync(speicherDatei);

            // Bei Fehler in Datei oder beim Auslesen wird null zurückgegeben.
            if (text != null)
            {
                ausDatei = JsonConvert.DeserializeObject<UserEinstellungen>(text);
            }
            else
            {
                ausDatei = null;
            }

            return ausDatei;
        }


        /// <summary>
        /// Speichert die übergebenen Partyliste in die dafür vorgesehene Datei. Falls die Datei schon bestehen sollte, wird diese Überschrieben.
        /// </summary>
        /// <param name="partyListe">Partyliste zu speichern</param>
        /// <returns></returns>
        public static async Task<bool> PartysSpeichern(IEnumerable<Party> partyListe)
        {
            bool erfolg = false;

            // Zieldatei erstellen/überschreiben
            speicherDatei = await speicherOrdner.CreateFileAsync(DateiPartys, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                string partyListeAlsJson = JsonConvert.SerializeObject(partyListe);
                // In Datei schreiben
                await FileIO.WriteTextAsync(speicherDatei, partyListeAlsJson);

                erfolg = true;
            }

            catch (Exception)
            {
                var message = new MessageDialog(Meldungen.DatenVerarbeitung.FehlerSpeichern, "Fehler beim Speichern!");
                await message.ShowAsync();
                return erfolg;
            }

            return erfolg;
        }

        /// <summary>
        /// Liest die Daten aus der Datei für die Partys.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Party>> PartysAuslesen()
        {
            IEnumerable<Party> partyListe = null;
            speicherDatei = await speicherOrdner.GetFileAsync(DateiPartys);

            // Daten auslesen
            string text = await FileIO.ReadTextAsync(speicherDatei);

            // Bei Fehler in Datei oder beim Auslesen wird null zurückgegeben.
            if (text != "null")
            {
                partyListe = JsonConvert.DeserializeObject<IEnumerable<Party>>(text);
            }
            else
            {
                partyListe = null;
            }

            return partyListe;

        }
    }
}
