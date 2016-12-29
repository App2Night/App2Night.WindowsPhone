﻿using System;
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
            progressRingAnmeldung.Visibility = Visibility.Collapsed;
        }

        private void Zurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        private async void Anmelden_WechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            bool korrekteEingabe = false;
            bool speichernErfolgreich = false;
            bool einstellungenErfolgreich = false;

            UserEinstellungen einst = new UserEinstellungen();
            einst.Radius = 50;

            anmeldung.Username = txtBoxAnmNUTZERNAME.Text;
            anmeldung.Email = txtBlAnmEMAIL.Text;
            anmeldung.Password = pwBoxPASSWORT.Password;

            progressRingAnmeldung.Visibility = Visibility.Visible;
            this.IsEnabled = false;            

            korrekteEingabe = await DatenVerarbeitung.LoginUeberpruefen(anmeldung);

            if (korrekteEingabe == true)
            {
                speichernErfolgreich = await DatenVerarbeitung.LoginSpeichern(anmeldung);

                // Default-Radius für Suchumfeld in Datei speichern (momentan nur Radius, hier stehen könnten noch weitere Einstellungen vom User gespeichert werden
                einstellungenErfolgreich = await DatenVerarbeitung.UserEinstellungenSpeichern(einst);

                if (speichernErfolgreich == true && einstellungenErfolgreich == true)
                {
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

            this.IsEnabled = true;
            progressRingAnmeldung.Visibility = Visibility.Collapsed;

        }
    }
}
