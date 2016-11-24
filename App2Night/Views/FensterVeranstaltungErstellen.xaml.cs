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
        public Party partyZuErstellen;

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

                Location loc = new Location();
                loc.CityName = textBoxErstellenORT.Text;
                loc.StreetName = textBoxErstellenADRESSE.Text; 
                loc.HouseNumber = textBoxErstellenHAUSNUMMER.Text;
                loc.Zipcode = textBoxErstellenPLZ.Text;

                partyZuErstellen.Location = loc;

                DateTime zwischenSpeicherDate = new DateTime(DatePickerErstellenDATUM.Date.Year, DatePickerErstellenDATUM.Date.Month, DatePickerErstellenDATUM.Date.Day,
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
