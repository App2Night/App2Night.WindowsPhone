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

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungErstellen : Page
    {
        public Party party = new Party();

        public FensterVeranstaltungErstellen()
        {
            this.InitializeComponent();

        }

        private void btnAbbrechen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private async void  btnWeiter_wechselZuErstellen02(object sender, RoutedEventArgs e)
        {
            party.PartyName = textBoxErstellenNAME.Text;
            party.Location.CityName = textBoxErstellenORT.Text;
            party.Location.HouseNumber = textBoxErstellenHAUSNUMMER.Text;
            party.Location.StreetName = textBoxErstellenADRESSE.Text;
            party.Location.Zipcode = textBoxErstellenPLZ.Text;
            party.PartyDate.Date = DatePickerErstellenDATUM.Date;
            party.PartyDate.TimeOfDay = TimePickerErstellenUHRZEIT.Time; 


            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen02), party);
        }
    }
}
