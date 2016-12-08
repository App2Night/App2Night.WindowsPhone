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
using App2Night.Controller;
using Newtonsoft.Json;
using App2Night.ModelsEnums.Model;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungAnzeigen : Page
    {
        public Party uebergebenderParameter = new Party();

        public FensterVeranstaltungAnzeigen()
        {
            this.InitializeComponent();
        }

        private void btnVormerken_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            //Ansicht des Reigsters vorgemerkt?
            this.Frame.Navigate(typeof(FensterHauptansicht));
            
        }

        private void btnZurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
            
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uebergebenderParameter = e.Parameter as Party;

            DateTime partyDatumUhrzeit = uebergebenderParameter.PartyDate;
            DateTime partyDatum = partyDatumUhrzeit.Date;
            TimeSpan partyUhrzeit = partyDatumUhrzeit.TimeOfDay;
            
            // null möglich!
            txtBlVeranstAnzeigenNAME.Text = uebergebenderParameter.PartyName;
            textBoxAnzeigenDATUM.Text = partyDatum.ToString("dd/MM/yyyy");
            textBoxAnzeigenUHRZEIT.Text = partyDatumUhrzeit.ToString("HH:mm");
            textBoxAnzeigenORT.Text = uebergebenderParameter.Location.CityName;
            textBoxAnzeigenMUSIKRICHTUNG.Text = uebergebenderParameter.MusicGenre.ToString();
            textBoxAnzeigenWeitereINFOS.Text = uebergebenderParameter.Description;
        }

        private void btnAufKarteAnzeigen_wechselZuKartenAnzeige(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterKartenansicht), uebergebenderParameter);
        }
    }
}
