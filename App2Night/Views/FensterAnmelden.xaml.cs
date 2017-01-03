using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App2Night.ModelsEnums.Model;
using App2Night.Logik;
using Windows.UI.Popups;
using App2Night.Ressources;
using System.Collections.Generic;

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

            // UserEinstellungen auf Default zurücksetzen
            UserEinstellungen einst = new UserEinstellungen();
            einst.Radius = 50;

            // "Neue" Werte speichern
            speichernErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

            // Partyliste aus Zwischenspeichern löschen
            IEnumerable<Party> liste = null;
            speichernErfolgreich = await DatenVerarbeitung.PartysSpeichern(liste);

            anmeldung.Username = txtBoxAnmNUTZERNAME.Text;
            anmeldung.Email = txtBlAnmEMAIL.Text;
            anmeldung.Password = pwBoxPASSWORT.Password;

            // Sperren der Ansicht
            this.IsEnabled = false;
            progressRingAnmeldung.Visibility = Visibility.Visible;          

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
                    var message = new MessageDialog(Meldungen.Anmeldung.Erfolg, "Erfolg!");
                    await message.ShowAsync();
                    this.Frame.Navigate(typeof(FensterHauptansicht)); 
                }
                else
                {
                    var message = new MessageDialog(Meldungen.Anmeldung.UnbekannterFehler, "Unbekannter Fehler!");
                    await message.ShowAsync();
                }
            }
            else
            {
                var message = new MessageDialog(Meldungen.Anmeldung.Misserfolg, "Fehler bei der Anmeldung!");
                await message.ShowAsync();
            }

            // Oberfläche entsperren
            progressRingAnmeldung.Visibility = Visibility.Collapsed;
            this.IsEnabled = true;

        }
    }
}
