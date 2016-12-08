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
using Windows.UI.Popups;
using App2Night.Logik;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterReg : Page
    {
        Login neuerNutzer = new Login();

        public FensterReg()
        {
            this.InitializeComponent();
        }

        private void btnZurueck_wechselZuAnmOderReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        private async void btnNutzerAnlegen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            neuerNutzer.Username = textBoxRegNUTZERNAME.Text;
            neuerNutzer.Email = textBoxRegEMAIL.Text;

            progRingReg.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            if (pwBoxPASSWORT.Password == pwBoxPASSWORTBEST.Password)
            {
                neuerNutzer.Password = pwBoxPASSWORTBEST.Password;
                bool status = await BackEndComUserLogik.CreateUser(neuerNutzer);

                progRingReg.Visibility = Visibility.Collapsed;

                if (status == true)
                {
                    var message = new MessageDialog($"Eine E-Mail mit Aktivierungslink wurde an die angegebene E-Mailadresse({neuerNutzer.Email}) geschickt.", "Nutzer erfolgreich registriert!");
                    await message.ShowAsync();

                    // Speichert Login und Token in Textdatei
                    await DatenVerarbeitung.DatenInDateiSchreibenLogin(neuerNutzer);

                    this.Frame.Navigate(typeof(FensterHauptansicht)); 
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

            this.IsEnabled = true;
        }
    }
}
