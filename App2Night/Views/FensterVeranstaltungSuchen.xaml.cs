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

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungSuchen: Page
    {
        public FensterVeranstaltungSuchen()
        {
            //DateTimeOffset aktuellesJahr = DateTime.Today.AddYears(0);
            //DateTimeOffset aktuellesJahrPlusEins = DateTime.Today.AddYears(1);

            this.InitializeComponent();
            //this.DatePickerVeranstSuche.Date = DateTime.Today;
            //this.DatePickerVeranstSuche.MinYear = aktuellesJahr;
            //this.DatePickerVeranstSuche.MaxYear = aktuellesJahrPlusEins;

        }

        private void btnAbbrechen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private void btnSuchen_wechselZuHauptansichtMitSuchergebnissen(object sender, RoutedEventArgs e)
        {
            // In der Anzeige müssen nun die Ergebnisse der Suche angezeigt werden
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private void chkBoxGPSNutzenClick_GPSinOrtWandeln(object sender, RoutedEventArgs e)
        {
            if (chkBoxSucheGPSNutzen.IsChecked == true)
            {
                // Keine Eingabe des Standortes zulassen
                textBoxEingabePLZ.IsReadOnly = true;
                textBoxEingabeOrt.IsReadOnly = true;
                Controller.FensterSucheController.GPSinOrtUmwandeln(); 
            }
            else //if (chkBoxSucheGPSNutzen.IsChecked == false)
            {
                // Eingabe des Standortes zulassen
                textBoxEingabePLZ.IsReadOnly = false;
                textBoxEingabeOrt.IsReadOnly = false;
            }

            //if (chkBoxErstellenGPS.IsChecked == true)
            //{
            //    var pos = GetGeoLocation.GetLocation().Result;
            //    party.Location.Latitude = pos.Coordinate.Latitude;
            //    party.Location.Longitude = pos.Coordinate.Longitude;
            //}
            //else
            //{
            //    party.Location.CityName = textBoxErstellenORT.Text;
            //    party.Location.StreetName = textBoxErstellenADRESSE.Text;
            //    party.Location.HouseNumber = textBoxErstellenHAUSNUMMER.Text;
            //}

        }
    }
}
