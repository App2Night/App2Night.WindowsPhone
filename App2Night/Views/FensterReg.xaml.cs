using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App2Night.ModelsEnums.Model;
using Windows.UI.Popups;
using App2Night.Logik;
using App2Night.Ressources;

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
            bool status = false;
            bool erfolg = false;
            bool speichernErfolgreich = false;

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
                status = await BackEndComUserLogik.CreateUser(neuerNutzer);

                progRingReg.Visibility = Visibility.Collapsed;

                // Abhängig vom Erfolg/Misserfolg beim Erstellen wird eine Nachricht angezeigt und ggf. die Ansicht gewechselt.
                if (status == true)
                {
                    var message = new MessageDialog(Meldungen.Registrierung.ErfolgEins + neuerNutzer.Email.ToString() + Meldungen.Registrierung.ErfolgZwei, "Nutzer erfolgreich registriert!");
                    await message.ShowAsync();

                    // Speichern der Login-Daten
                    erfolg = await DatenVerarbeitung.LoginSpeichern(neuerNutzer);

                    // UserEinstellungen auf Default zurücksetzen
                    UserEinstellungen einst = new UserEinstellungen();
                    einst.Radius = 50;
                    einst.GPSErlaubt = false;

                    // "Neue" Werte speichern
                    speichernErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

                    if (erfolg == true && speichernErfolgreich == true)
                    {
                        // Wechsel zur Hauptansicht
                        this.Frame.Navigate(typeof(FensterHauptansicht)); 
                    }
                    else
                    {
                        message = new MessageDialog(Meldungen.Registrierung.SpeicherFehler, "Fehler beim Speichern!");
                        await message.ShowAsync();
                    }
                }
                else
                {
                    var message = new MessageDialog(Meldungen.Registrierung.ErstellenFehler, "Fehler beim Erstellen!");
                    await message.ShowAsync();
                }
            }
            else
            {
                var message = new MessageDialog(Meldungen.Registrierung.PasswortFehler, "Passwörter sind nciht identisch!");
                await message.ShowAsync();
            }

            // Entsperren der Oberfläche
            this.IsEnabled = true;
        }
    }
}
