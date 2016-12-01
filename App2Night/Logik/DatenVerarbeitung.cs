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
        public static ApplicationDataContainer speicherOrt = ApplicationData.Current.LocalSettings;
        private static string DateiLogin = "Login.txt";
        private static string DateiToken = "Token.txt";

        public static async Task<bool> DatenInDateiSchreibenLogin(Login neuerNutzer)
        {
            bool erfolg = false;

            //StorageFolder speicherOrdner = ApplicationData.Current.LocalFolder;
            //StorageFile speicherDatei = await speicherOrdner.CreateFileAsync(DateiLogin, Windows.Storage.CreationCollisionOption.ReplaceExisting);


            StorageFolder speicherOrdner = ApplicationData.Current.LocalFolder;

            ApplicationDataCompositeValue daten = new ApplicationDataCompositeValue();

            daten["Userame"] = neuerNutzer.Username;
            daten["Email"] = neuerNutzer.Email;
            daten["Password"] = neuerNutzer.Password;

            speicherOrt.Values["Login"] = daten;

            if (speicherOrt.Values.Count != 0)
            {
                erfolg = true;
            }
            else
            {
                var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
                await message.ShowAsync();
                erfolg = false;
            }

            //// Kontrolle
            //if (speicherOrdner == null)
            //{
            //    var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
            //    await message.ShowAsync();
            //    return erfolg;
            //}

            //try
            //{
            //    // Datei anlegen, falls vorhanden neu erstellen
            //    var stream = await speicherDatei.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            //    using (var outputStream = stream.GetOutputStreamAt(0))
            //    {
            //        using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
            //        {
            //            string loginJsonAlsString = JsonConvert.SerializeObject(neuerNutzer);

            //            // speichern 
            //            await dataWriter.StoreAsync();
            //            await outputStream.FlushAsync();
            //        }
            //    }
            //    // Stream zum Schreiben beenden
            //    stream.Dispose();
            //    erfolg = true;
            //}

            //catch (Exception ex)
            //{
            //    var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
            //    await message.ShowAsync();
            //    return erfolg;
            //}
            return erfolg;
        }

        public static async Task<Login> DatenAusDateiLesenLogin()
        {
            Login ausDatei = new Login();
            //StorageFolder speicherOrdner = Windows.Storage.ApplicationData.Current.LocalFolder;
            //StorageFile speicherDatei = await speicherOrdner.GetFileAsync(DateiToken);

            //string text = await Windows.Storage.FileIO.ReadTextAsync(speicherDatei);

            //ausDatei = JsonConvert.DeserializeObject<Login>(text);

            ApplicationDataCompositeValue datenAusSpeicher = (ApplicationDataCompositeValue)speicherOrt.Values["exampleCompositeSetting"];

            if (datenAusSpeicher == null)
            {
                // No data
            }
            else
            {
                ausDatei.Username = datenAusSpeicher["Username"].ToString();
                ausDatei.Password = datenAusSpeicher["Password"].ToString();
                ausDatei.Email = datenAusSpeicher["Email"].ToString();
            }

            return ausDatei;
        }

        public static async Task<bool> DatenInDateiSchreibenToken(Login neuerNutzer)
        {
            bool erfolg = false;
            StorageFolder speicherOrdner = ApplicationData.Current.LocalFolder;
            StorageFile speicherDatei = await speicherOrdner.CreateFileAsync(DateiLogin, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // Kontrolle
            if (speicherOrdner == null)
            {
                var message = new MessageDialog("Es ist ein Problem aufgetreten. Bitte versuche es später erneut.");
                await message.ShowAsync();
                return erfolg;
            }

            try
            {
                // Datei anlegen, falls vorhanden neu erstellen
                var stream = await speicherDatei.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                    {
                        string loginJsonAlsString = JsonConvert.SerializeObject(neuerNutzer);

                        // speichern 
                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
                // Stream zum Schreiben beenden
                stream.Dispose();
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
            StorageFolder speicherOrdner = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile speicherDatei = await speicherOrdner.GetFileAsync(DateiLogin);

            string text = await Windows.Storage.FileIO.ReadTextAsync(speicherDatei);

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


    }
}
