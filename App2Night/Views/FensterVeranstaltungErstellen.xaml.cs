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
using App2Night.Logik;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungErstellen : Page
    {
        public CreatePartyModel partyZuErstellen;

        public FensterVeranstaltungErstellen()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            partyZuErstellen = e.Parameter as CreatePartyModel;

            // Erst anzeigen, wenn bereits Werte vorhanden sind (man kommt von Erstellen02)
            if (partyZuErstellen != null)
            {
                textBoxErstellenNAME.Text = partyZuErstellen.PartyName;
                textBoxErstellenORT.Text = partyZuErstellen.CityName;
                textBoxErstellenHAUSNUMMER.Text = partyZuErstellen.HouseNumber;
                textBoxErstellenADRESSE.Text = partyZuErstellen.StreetName;
                textBoxErstellenPLZ.Text = partyZuErstellen.ZipCode;
                DatePickerErstellenDATUM.Date = new DateTime(partyZuErstellen.PartyDate.Date.Day, partyZuErstellen.PartyDate.Month, partyZuErstellen.PartyDate.Year); 
            }
        }

        private void btnAbbrechen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private async void  btnWeiter_wechselZuErstellen02(object sender, RoutedEventArgs e)
        {
            CreatePartyModel partyZuErstellen = new CreatePartyModel();

            // Validieren der Ortsangabe
            Location zuValidieren = new Location();
            zuValidieren.CityName = textBoxErstellenORT.Text;
            zuValidieren.StreetName = textBoxErstellenADRESSE.Text;
            zuValidieren.HouseNumber = textBoxErstellenHAUSNUMMER.Text;
            zuValidieren.ZipCode = textBoxErstellenPLZ.Text;

            Token tok = await DatenVerarbeitung.aktuellerToken();

            // TODO: Prüfen, ob das geht
            string erfolg = await BackEndComPartyLogik.ValidateLocation(zuValidieren, tok);

            //TODO: Nullwerte abfangen
            //TODO: Auf falsche Eingabe reagieren 
            //TODO: bei Zurueckkommen auf Erstellen02 müssen die Werte noch da sein
            try
            {
                partyZuErstellen.PartyName = textBoxErstellenNAME.Text;

                partyZuErstellen.CityName = textBoxErstellenORT.Text;
                partyZuErstellen.StreetName = textBoxErstellenADRESSE.Text;
                partyZuErstellen.HouseNumber = textBoxErstellenHAUSNUMMER.Text;
                partyZuErstellen.ZipCode = textBoxErstellenPLZ.Text;

                DateTime zwischenSpeicherDate = new DateTime(DatePickerErstellenDATUM.Date.Year, DatePickerErstellenDATUM.Date.Month, DatePickerErstellenDATUM.Date.Day,
                                                                                        TimePickerErstellenUHRZEIT.Time.Hours, TimePickerErstellenUHRZEIT.Time.Minutes, TimePickerErstellenUHRZEIT.Time.Seconds);

                partyZuErstellen.PartyDate = zwischenSpeicherDate;

                if (partyZuErstellen.PartyDate < DateTime.Today)
                {
                    Exception FehlerhaftesDatum = new Exception();
                    throw FehlerhaftesDatum;
                }
            }
            catch (Exception ex)
            {
                var message = new MessageDialog("Fehler! Ein oder mehrere Eingaben sind ungültig!\nBeispielsweise wird eine Party in der Vergangenheit angelegt oder die Adresse existiert nicht!");
                await message.ShowAsync();
                return;
            }

            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen02), partyZuErstellen);
        }
    }
}
