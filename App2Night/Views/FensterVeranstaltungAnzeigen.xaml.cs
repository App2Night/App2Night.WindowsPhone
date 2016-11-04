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
using static App2Night.APIObjects.Party;
using App2Night.APIObjects;

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

            // null möglich!
            txtBlVeranstAnzeigenNAME.Text = uebergebenderParameter.PartyName;
            textBoxAnzeigenDATUM.Text = uebergebenderParameter.PartyDate;
            textBoxAnzeigenORT.Text = uebergebenderParameter.Location.CityName;
            //TODO: string von PartyDate in Uhrzeit und Datum aendern
            //textBoxAnzeigenDATUM.Text = uebergebenderParameter.PartyDate;
            //textBoxAnzeigenUHRZEIT.Text = uebergebenderParameter.PartyDate;
            textBoxAnzeigenWeitereINFOS.Text = uebergebenderParameter.Description;
        }


    }
}
