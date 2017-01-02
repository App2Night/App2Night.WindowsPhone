using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App2Night.ModelsEnums.Model;
using App2Night.Logik;
using Windows.UI.Popups;

namespace App2Night.Views
{
    /// <summary>
    /// Dies ist die Seite für die Anmeldung. Hier werden die Daten vom Nutzer validiert. Falls diese korrekt sind, wird der Nutzer auf die Hauptansicht 
    /// weitergeleitet.
    /// </summary>
    public sealed partial class FensterAnmelden : Page
    {
        public Login anmeldung = new Login();

        public FensterAnmelden()
        {
            this.InitializeComponent();
            progressRingAnmeldung.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Einfacher Wechsel zu FensterAnmOdReg (Zurück).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zurueck_wechselZuAnmOderReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        /// <summary>
        /// Hier werden die Daten vom Nutzer validiert. Entweder ändert sich die Anzeige (korrekte Daten) oder der Nutzer erhält eine Nachricht, 
        /// dass die eingegeben Daten falsch sind. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Anmelden_WechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            bool korrekteEingabe = false;
            bool speichernErfolgreich = false;
            bool einstellungenErfolgreich = false;

            // Einstellungen vom User auf einen Standardwert setzen.
            UserEinstellungen einst = new UserEinstellungen();
            einst.Radius = 50;

            anmeldung.Username = txtBoxAnmNUTZERNAME.Text;
            anmeldung.Email = txtBlAnmEMAIL.Text;
            anmeldung.Password = pwBoxPASSWORT.Password;

            // Sperren der Ansicht
            progressRingAnmeldung.Visibility = Visibility.Visible;
            this.IsEnabled = false;            

            korrekteEingabe = await DatenVerarbeitung.LoginUeberpruefen(anmeldung);

            if (korrekteEingabe == true)
            {
                // Speichern der Anmeldedaten in eine Textdatei
                speichernErfolgreich = await DatenVerarbeitung.LoginSpeichern(anmeldung);

                // Default-Radius für Suchumfeld in Datei speichern 
                einstellungenErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

                if (speichernErfolgreich == true && einstellungenErfolgreich == true)
                {
                    // Wenn alles erfolgreich gespeichert wurde
                    progressRingAnmeldung.Visibility = Visibility.Collapsed;
                    var message = new MessageDialog("Erfolgreich angemeldet. Viel Spaß!", "Erfolg!");
                    await message.ShowAsync();
                    this.Frame.Navigate(typeof(FensterHauptansicht)); 
                }
                else
                {
                    var message = new MessageDialog("Es ist ein unerwarteter Fehler aufgetreten. Bitte versuche es später erneut.", "Unbekannter Fehler!");
                    await message.ShowAsync();
                }
            }
            else
            {
                var message = new MessageDialog("Keine korrekten Nutzerdaten oder der Aktivierungslink wurde noch nicht bestätigt!", "Fehler bei der Anmeldung!");
                await message.ShowAsync();
            }

            // Oberfläche entsperren
            this.IsEnabled = true;
            progressRingAnmeldung.Visibility = Visibility.Collapsed;

        }
    }
}
