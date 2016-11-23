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
using Windows.Devices.Geolocation;
using App2Night.Controller;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungErstellen : Page
    {
        public Party partyZuErstellen = new Party();

        public FensterVeranstaltungErstellen()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            partyZuErstellen = e.Parameter as Party;

            // Erst anzeigen, wenn bereits Werte vorhanden sind (man kommt von Erstellen02)
            if (partyZuErstellen != null)
            {
                textBoxErstellenNAME.Text = partyZuErstellen.PartyName;
                textBoxErstellenORT.Text = partyZuErstellen.Location.CityName;
                textBoxErstellenHAUSNUMMER.Text = partyZuErstellen.Location.HouseNumber;
                textBoxErstellenADRESSE.Text = partyZuErstellen.Location.StreetName;
                textBoxErstellenPLZ.Text = partyZuErstellen.Location.Zipcode;
                DatePickerErstellenDATUM.Date = new DateTime(partyZuErstellen.PartyDate.Date.Day, partyZuErstellen.PartyDate.Month, partyZuErstellen.PartyDate.Year); 
            }
        }

        private void btnAbbrechen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private void  btnWeiter_wechselZuErstellen02(object sender, RoutedEventArgs e)
        {
            Party partyZuErstellen = new Party();
            //TODO: Nullwerte abfangen
            //TODO: Auf falsche Eingabe reagieren 
            //TODO: bei Zurueckkommen auf Erstellen02 müssen die Werte noch da sein
            try
            {
                partyZuErstellen.PartyName = textBoxErstellenNAME.Text;
                partyZuErstellen.Location.CityName = textBoxErstellenORT.Text;
                partyZuErstellen.Location.HouseNumber = textBoxErstellenHAUSNUMMER.Text;
                partyZuErstellen.Location.StreetName = textBoxErstellenADRESSE.Text;
                partyZuErstellen.Location.Zipcode = textBoxErstellenPLZ.Text;
                DateTime zwischenSpeicherDate = new DateTime(DatePickerErstellenDATUM.Date.Day, DatePickerErstellenDATUM.Date.Month, DatePickerErstellenDATUM.Date.Year,
                                                                                        TimePickerErstellenUHRZEIT.Time.Hours, TimePickerErstellenUHRZEIT.Time.Minutes, TimePickerErstellenUHRZEIT.Time.Seconds);
                partyZuErstellen.PartyDate = zwischenSpeicherDate;
            }
            catch (Exception)
            {
                var message = new MessageDialog("Fehler! Ein oder mehrere Eingaben sind ungültig!");
                message.ShowAsync();
            }

            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen02), partyZuErstellen);
        }
    }
}
