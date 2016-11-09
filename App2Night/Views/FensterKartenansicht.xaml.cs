using App2Night.APIObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
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
    public sealed partial class FensterKartenansicht : Page
    {
        public Party uebergebenderParameter = new Party();

        public FensterKartenansicht()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uebergebenderParameter = e.Parameter as Party;

            // Specify a known location.
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = uebergebenderParameter.Location.Latitude, Longitude = uebergebenderParameter.Location.Longitude };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set the map location.
            mapControlKarte.Center = cityCenter;
            mapControlKarte.ZoomLevel = 15;
            mapControlKarte.LandmarksVisible = true;
        }

        private void btnKartenAnsichtZurueck_WechselZuPartyAnzeigen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen), uebergebenderParameter);
        }
    }
}
