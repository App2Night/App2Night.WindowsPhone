using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App2Night.Logik;
using App2Night.ModelsEnums.Model;
using Windows.UI.Popups;
using App2Night.Ressources;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace App2Night.Views
{
    /// <summary>
    /// Hier werden Einstellungen vom Nutzer angezeigt/angenommen und gespeichert.
    /// </summary>
    public sealed partial class FensterUserEinstellungen : Page
    {
        public FensterUserEinstellungen()
        {
            this.InitializeComponent();
            progRingUserEinstellungen.Visibility = Visibility.Collapsed;
            progRingUserEinstellungen.IsActive = false;

            // Zeigt den aktuellen, vom Nutzer gesetzten Radius an.
            SuchRadiusUndGPSEinstellen();

        }

        /// <summary>
        /// Auslesen des vom Nutzer gesetzten Suchradius aus einer Datei und Anzeigen durch Slider.
        /// </summary>
        private async void SuchRadiusUndGPSEinstellen()
        {
            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();

            if (einst.Radius != 0)
            {
                sliderSuchradius.Value = einst.Radius;
            }
            else
            {
                sliderSuchradius.Value = 50;
            }

            if (einst.GPSErlaubt == true)
            {
                toggleSwitchGPSErlaubnis.IsOn = true;
            }
            else
            {
                toggleSwitchGPSErlaubnis.IsOn = false;
            }
        }

        /// <summary>
        /// Einfacher Wechsel zur Seite Hauptansicht. Nicht-gespeicherte Daten gehen verloren.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Abmelden durch Wechsel zur Startansicht (FensterAnmOdReg) und Speichern von Defaultwerten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAbmelden_wechselZuFensterAnmelden(object sender, RoutedEventArgs e)
        {
            // Sperren der Oberfläche
            SperrenDerAnsicht();

            // UserEinstellungen auf Default zurücksetzen
            bool speichernErfolgreich = false;
            UserEinstellungen einst = new UserEinstellungen();
            einst.Radius = 50;

            // "Neue" Werte speichern
            speichernErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

            // Partyliste aus Zwischenspeichern löschen
            IEnumerable<Party> liste = null;
            speichernErfolgreich = await DatenVerarbeitung.PartysSpeichern(liste);

            // Oberfläche entsperren
            EntsperrenDerAnsicht();

            // Wechsel zum Start
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        /// <summary>
        /// Wechsel zur Hauptansicht und Speichern der eingegeben Daten.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Speichern_DatenSichernUndWechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            // Sperren der Oberfläche
            SperrenDerAnsicht();

            bool speichernErfolgreich = false;

            UserEinstellungen einst = new UserEinstellungen();
            // Aktueller Wert des Sliders für den Radius
            einst.Radius = (float)sliderSuchradius.Value;
            einst.GPSErlaubt = toggleSwitchGPSErlaubnis.IsOn;
            // Weitere Einstellungen möglich

            // Speichern der Nutzereinstellungen in einer Datei
            speichernErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

            // Wechsel zur Hauptansicht und Ausgabe des Erfolgs/Misserfolgs beim Speichern
            if (speichernErfolgreich == true)
            {
                this.Frame.Navigate(typeof(FensterHauptansicht));
            }
            else
            {
                var message = new MessageDialog(Meldungen.UserEinstellungen.SpeicherFehler, "Fehler!");
                await message.ShowAsync();
                this.Frame.Navigate(typeof(FensterHauptansicht));
            }

            // Oberfläche entsperren
            EntsperrenDerAnsicht();
        }

        /// <summary>
        /// Zeigt an, vom wem die App entwickelt wurde und welche Bibliotheken von Dritten verwendet wurden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void About_zeigeAbout(object sender, RoutedEventArgs e)
        {
            var message = new MessageDialog(Meldungen.UserEinstellungen.About, "App2Night");
            await message.ShowAsync();
        }

        /// <summary>
        /// Zeigt die E-Mailadresse des Supports an.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Email_zeigeKontakt(object sender, RoutedEventArgs e)
        {
            var message = new MessageDialog(Meldungen.UserEinstellungen.EmailKontakt, "App2Night");
            await message.ShowAsync();
        }

        /// <summary>
        /// Stellt die Erlaubnis für die GPS-Nutzung ein und fragt bei Änderungen nochmal nach.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toggleSwitchGPSErlaubnis_GPSEinstellen(object sender, RoutedEventArgs e)
        {
            // Oberfläche sperren
            SperrenDerAnsicht();

            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();

            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                // Status des ToggleSwitches NACH dem Antippen
                if (toggleSwitchGPSErlaubnis.IsOn == true)
                {
                    einst.GPSErlaubt = true;
                }
                else
                {
                    einst.GPSErlaubt = false;
                }
            }
            else
            {
                einst.GPSErlaubt = false;
                toggleSwitchGPSErlaubnis.IsOn = false; // Da GPS nicht erlaubt
                var message = new MessageDialog(Meldungen.UserEinstellungen.FehlerGPS, "Achtung!");
                await message.ShowAsync();
            }

            // Neue Einstellungen speichern
            await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

            // Oberfläche entsperren
            EntsperrenDerAnsicht();
        }

        /// <summary>
        /// Sperrt die Oberfläche.
        /// </summary>
        private void SperrenDerAnsicht()
        {
            progRingUserEinstellungen.Visibility = Visibility.Visible;
            progRingUserEinstellungen.IsActive = true;
            this.AppBarButtonAbout.IsEnabled = false;
            this.AppBarButtonEmail.IsEnabled = false;
            this.AppBarButtonUserInfoSpeichern.IsEnabled = false;
            this.AppBarButtonUserInfoZurueck.IsEnabled = false;
            this.IsEnabled = false;
        }

        /// <summary>
        /// Entsperrt die Oberfläche.
        /// </summary>
        private void EntsperrenDerAnsicht()
        {
            this.IsEnabled = true;
            this.AppBarButtonAbout.IsEnabled = true;
            this.AppBarButtonEmail.IsEnabled = true;
            this.AppBarButtonUserInfoSpeichern.IsEnabled = true;
            this.AppBarButtonUserInfoZurueck.IsEnabled = true;
            progRingUserEinstellungen.Visibility = Visibility.Collapsed;
            progRingUserEinstellungen.IsActive = false;
        }
    }
}
