using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App2Night.Logik;
using App2Night.ModelsEnums.Model;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterUserEinstellungen : Page
    {
        public FensterUserEinstellungen()
        {
            this.InitializeComponent();
            progRingUserEinstellungen.Visibility = Visibility.Collapsed;
        }

        private void Zurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private async void btnAbmelden_wechselZuFensterAnmelden(object sender, RoutedEventArgs e)
        {
            progRingUserEinstellungen.Visibility = Visibility.Visible;
            this.IsEnabled = false;
            // UserEinstellungen auf Default zurücksetzen
            bool speichernErfolgreich = false;
            UserEinstellungen einst = new UserEinstellungen();
            einst.Radius = 50.00;

            speichernErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

            this.IsEnabled = true;
            progRingUserEinstellungen.Visibility = Visibility.Collapsed;

            this.Frame.Navigate(typeof(FensterAnmelden));
        }

        private async void Speichern_DatenSichernUndWechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            progRingUserEinstellungen.Visibility = Visibility.Visible;
            this.IsEnabled = false;
            bool speichernErfolgreich = false;
            UserEinstellungen einst = new UserEinstellungen();
            einst.Radius = sliderSuchradius.Value;
            // Weitere Einstellungen möglich...

            speichernErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

            if (speichernErfolgreich == true)
            {
                this.Frame.Navigate(typeof(FensterHauptansicht));
            }
            else
            {
                var message = new MessageDialog("Leider ist ein Fehler beim Speichern der Daten aufgetreten.", "Fehler!");
                await message.ShowAsync();
                this.Frame.Navigate(typeof(FensterHauptansicht));
            }

            this.IsEnabled = true;
            progRingUserEinstellungen.Visibility = Visibility.Collapsed;
        }

        private async void About_zeigeAbout(object sender, RoutedEventArgs e)
        {
            string text = "Diese App wurde entwickelt von Yvette Labastille und Manuela Leopold im Zuge einer Vorlesung an der DHBW Stuttgart Campus Horb.\n" +
                                "Verwendet wurde:\n" +
                                "- JSON-Framwork von Newtonsoft\n" +
                                "- Xam.Plugin.Geolocator von James Montemango\n" + 
                                "- Bibliotheken vom Microsoft";
            var message = new MessageDialog(text, "App2Night");
            await message.ShowAsync();
        }

        private async void Email_zeigeKontakt(object sender, RoutedEventArgs e)
        {
            var message = new MessageDialog("Schreib uns doch unter mobApp@outlook.com", "App2Night");
            await message.ShowAsync();
        }
    }
}
