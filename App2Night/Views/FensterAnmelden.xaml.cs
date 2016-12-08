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
using App2Night.ModelsEnums.Model;
using App2Night.Logik;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterAnmelden : Page
    {
        public Login anmeldung = new Login();

        public FensterAnmelden()
        {
            this.InitializeComponent();
        }

        private void btnZurueck_wechselZuAnmOderReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        

        private void btnPwVergessen_wechselZuNeuesPW(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterNeuesPW));
        }

        private void Zurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        private void PasswortVergessen_wechselZuNeuesPasswort(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterNeuesPW));
        }

        private async void Anmelden_WechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            anmeldung.Username = txtBoxAnmNUTZERNAME.Text;
            anmeldung.Email = txtBlAnmEMAIL.Text;
            anmeldung.Password = pwBoxPASSWORT.Password;

            this.IsEnabled = false;

            // TODO: GetUserInfos damit überprüfen statt mit gespeicherten Daten, da eventuell nicht aktuell. Wenn vorhanden, dann in Datei schreiben.
            bool korrekteEingabe = await DatenVerarbeitung.LoginUeberpruefen(anmeldung);

            if (korrekteEingabe == true)
            {
                this.Frame.Navigate(typeof(FensterHauptansicht));
            }
            else
            {
                var message = new MessageDialog("Keine korrekten Nutzerdaten!");
                await message.ShowAsync();
            }

            this.IsEnabled = true;

        }
    }
}
