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
using Windows.Data.Json;
using App2Night.APIObjects;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterHauptansicht : Page
    {
        public Party.RootObject partyRootObject = new Party.RootObject();

        public FensterHauptansicht()
        {
            this.InitializeComponent();
            ListView listViewSuchErgebnis = new ListView();
        }

        private void btnErstellen_wechselZuVeranstErstellen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen));
        }

        private void btnSuche_wechselZuVeranstSuchen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungSuchen));
        }

        private async void btnVeranstInDerNaehe_GetPartys(object sender, RoutedEventArgs e)
        {
            //Anzeige der Partys, die vom Server geschickt werden
            
            partyRootObject = await FensterHauptansichtController.DatenFromServerToParty();

            listViewSuchErgebnis.Items.Add(partyRootObject.PartyName);

        }

        private void listViewSuchErgebnisse_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Daten von der Party mitnehmen
            
            this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen));
        }

        private void listView_ClickOnItem(object sender, SelectionChangedEventArgs e)
        {
            // Seitenwechsel mit Übergabe der Daten aus der ausgewählten Party 
            this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen), partyRootObject);
            
        }



    }
}
