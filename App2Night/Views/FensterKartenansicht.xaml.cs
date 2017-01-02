using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using App2Night.ModelsEnums.Model;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;

namespace App2Night.Views
{
    /// <summary>
    /// Zeigt die Party auf der Karte an.
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

            // Festlegen der Position
            BasicGeoposition partyPosition = new BasicGeoposition() { Latitude = uebergebenderParameter.Location.Latitude, Longitude = uebergebenderParameter.Location.Longitude };
            Geopoint partyZentrum = new Geopoint(partyPosition);

            // Festlegen des Mittelpunkts
            mapControlKarte.Center = partyZentrum;
            mapControlKarte.ZoomLevel = 15;
            mapControlKarte.LandmarksVisible = true;

            // Icon für Standort Party
            MapIcon partyIcon = new MapIcon();
            partyIcon.Title = uebergebenderParameter.PartyName;
            partyIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("App2Night/Assets/Square150x150Logo.scale-400.png"));

        }

        private void KartenAnsichtZurueck_WechselZuPartyAnzeigen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen), uebergebenderParameter);
        }

        private void KartenAnsichtZurueck_WechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht), uebergebenderParameter);
        }
    }
}
