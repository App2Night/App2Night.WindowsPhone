using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App2Night.ModelsEnums.Model;
using Windows.UI.Popups;
using App2Night.Logik;

namespace App2Night.Views
{
    /// <summary>
    /// Dies ist das Fenster zum Nutzer anlegen.
    /// </summary>
    public sealed partial class FensterReg : Page
    {
        Login neuerNutzer = new Login();

        public FensterReg()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Einfacher Wechsel zu FensterAnmOdReg (Zurück).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zurueck_wechselnZuAnmOReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        /// <summary>
        /// Erstellen des neuen Nutzers und bei Erfolg wechseln zur Hauptansicht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bestaetigen_WechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            // Daten auslesen
            neuerNutzer.Username = textBoxRegNUTZERNAME.Text;
            neuerNutzer.Email = textBoxRegEMAIL.Text;

            // Oberfläche sperren
            progRingReg.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            if (pwBoxPASSWORT.Password == pwBoxPASSWORTBEST.Password)
            {
                // Speichern des Passworts
                neuerNutzer.Password = pwBoxPASSWORTBEST.Password;
                // Anlegen des neuen Nutzers
                bool status = await BackEndComUserLogik.CreateUser(neuerNutzer);

                progRingReg.Visibility = Visibility.Collapsed;

                // Abhängig vom Erfolg/Misserfolg beim Erstellen wird eine Nachricht angezeigt und ggf. die Ansicht gewechselt.
                if (status == true)
                {
                    var message = new MessageDialog($"Eine E-Mail mit Aktivierungslink wurde an die angegebene E-Mailadresse ({neuerNutzer.Email}) geschickt.", "Nutzer erfolgreich registriert!");
                    await message.ShowAsync();

                    // Speichern der Login-Daten
                    bool erfolg = await DatenVerarbeitung.LoginSpeichern(neuerNutzer);

                    if (erfolg == true)
                    {
                        // Wechsel zur Hauptansicht
                        this.Frame.Navigate(typeof(FensterHauptansicht)); 
                    }
                    else
                    {
                        message = new MessageDialog("Leider ist ein Fehler beim Speichern der Daten aufgetreten. Bitte versuche, die Anzumelden.", "Fehler beim Speichern!");
                        await message.ShowAsync();
                    }
                }
                else
                {
                    var message = new MessageDialog("Fehler bei Erstellen des Nutzers!");
                    await message.ShowAsync();
                }
            }
            else
            {
                var message = new MessageDialog("Fehler! Die Passwörter stimmen nicht überein!");
                await message.ShowAsync();
            }

            // Entsperren der Oberfläche
            this.IsEnabled = true;
        }
    }
}
