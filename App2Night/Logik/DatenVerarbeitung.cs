
using App2Night.ModelsEnums.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace App2Night.Logik
{
    class DatenVerarbeitung
    {
        private static StorageFolder speicherOrdner = ApplicationData.Current.LocalFolder;
        private static StorageFile speicherDatei;
        private static string DateiLogin = "Login.txt";
        private static string DateiToken = "Token.txt";

        public static async Task<bool> DatenInDateiSchreibenLogin(Login neuerNutzer)
        {
            bool erfolg = false;
            speicherDatei = await speicherOrdner.CreateFileAsync(DateiLogin, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                string loginJsonAlsString = JsonConvert.SerializeObject(neuerNutzer);
                await FileIO.WriteTextAsync(speicherDatei, loginJsonAlsString);

                erfolg = true;
            }

            catch (Exception ex)
            {
                var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
                await message.ShowAsync();
                return erfolg;
            }
            return erfolg;
        }

        public static async Task<Login> DatenAusDateiLesenLogin()
        {
            Login ausDatei = new Login();
            speicherDatei = await speicherOrdner.GetFileAsync(DateiLogin);

            string text = await FileIO.ReadTextAsync(speicherDatei);

            ausDatei = JsonConvert.DeserializeObject<Login>(text);

            return ausDatei;
        }

        public static async Task<bool> DatenInDateiSchreibenToken(Login neuerNutzer)
        {
            bool erfolg = false;
            speicherDatei = await speicherOrdner.CreateFileAsync(DateiToken, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                string loginJsonAlsString = JsonConvert.SerializeObject(neuerNutzer);
                await FileIO.WriteTextAsync(speicherDatei, loginJsonAlsString);

                erfolg = true;
            }

            catch (Exception ex)
            {
                var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
                await message.ShowAsync();
                return erfolg;
            }
            return erfolg;
        }

        public static async Task<Token> DatenAusDateiLesenToken()
        {
            Token ausDatei = new Token();
            speicherDatei = await speicherOrdner.GetFileAsync(DateiToken);

            string text = await FileIO.ReadTextAsync(speicherDatei);

            ausDatei = JsonConvert.DeserializeObject<Token>(text);

            return ausDatei;
        }

        public static async Task<bool> LoginUeberpruefen(Login loginZuPruefen)
        {
            bool korrekterLogin = false;

            Login ausDatei = await DatenAusDateiLesenLogin();

            if (ausDatei.Email == loginZuPruefen.Email && ausDatei.Password == loginZuPruefen.Password && ausDatei.Username == loginZuPruefen.Username)
            {
                korrekterLogin = true;
            }

            return korrekterLogin;
        }

        public static async Task<Token> aktuellerTokenFuerPost()
        {
            // TODO: File not found
            Login aktuellerNutzer = await DatenVerarbeitung.DatenAusDateiLesenLogin();
            Token aktuellerToken = await DatenVerarbeitung.DatenAusDateiLesenToken();

            return aktuellerToken = await BackEndComUserLogik.RefreshToken(aktuellerToken);
        }


    }
}
