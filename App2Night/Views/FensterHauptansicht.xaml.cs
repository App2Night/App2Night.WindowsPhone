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
using App2Night.ModelsEnums;
using App2Night.ModelsEnums.Model;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using App2Night.Logik;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterHauptansicht : Page
    {
        public Party party;
        public IEnumerable<Party> partyListe;

        public FensterHauptansicht()
        {
            this.InitializeComponent();
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;          
        }

        //private void btnErstellen_wechselZuVeranstErstellen(object sender, RoutedEventArgs e)
        //{
        //    this.Frame.Navigate(typeof(FensterVeranstaltungErstellen));
        //}

        private void btnSuche_wechselZuVeranstSuchen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungSuchen));
        }

        //public async void btnVeranstInDerNaehe_GetPartys(object sender, RoutedEventArgs e)
        //{
        //    this.IsEnabled = false;
        //    //Anzeige der Partys, die vom Server geschickt werden
        //    progressRingInDerNaehe.IsEnabled = true;
        //    progressRingInDerNaehe.Visibility = Visibility.Visible;

        //    partyListe =  await FensterHauptansichtController.btnInDerNaehePartysAbrufen();

        //    if (partyListe.Any())
        //    {
        //        int anzahl = partyListe.Count();

        //        for (int i = 0; i < anzahl; i++)
        //        {
        //            party = partyListe.ElementAt(i);
        //            listViewSuchErgebnis.Items.Add(party.PartyName);
        //        }
        //    }
        //    else
        //    {
        //        var message = new MessageDialog("Leider keine Partys in deiner Nähe.");
        //        await message.ShowAsync();
        //    }

        //    progressRingInDerNaehe.IsEnabled = false;
        //    progressRingInDerNaehe.Visibility = Visibility.Collapsed;

        //    this.IsEnabled = true;

        //}

        private void listViewSuchErgebnisse_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Daten von der Party mitnehmen
            
            this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen));
        }

        private void listView_ClickOnItem(object sender, SelectionChangedEventArgs e)
        {
            // Seitenwechsel mit Übergabe der Daten aus der ausgewählten Party 
            string partyName = ((ListView)sender).SelectedItem.ToString();
            bool partygefunden = false;
            int suchDurchLauf = 0;


            // TODO: geht da wirklich so?
            while (partygefunden == false)
            {
                party = partyListe.ElementAt(suchDurchLauf);

                if (party.PartyName == partyName)
                {
                    partygefunden = true;
                }
                else
                {
                    suchDurchLauf++;
                }
            }

            party = partyListe.ElementAt(suchDurchLauf);

            this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen), party);
            
        }

        

        private void Hinzufuegen_wechselZuErstellen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen));
        }

        public async void Suchen_abrufenPartys(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            //Anzeige der Partys, die vom Server geschickt werden
            progressRingInDerNaehe.IsEnabled = true;
            progressRingInDerNaehe.Visibility = Visibility.Visible;

            partyListe = await FensterHauptansichtController.btnInDerNaehePartysAbrufen();

            if (partyListe.Any())
            {
                int anzahl = partyListe.Count();

                for (int i = 0; i < anzahl; i++)
                {
                    party = partyListe.ElementAt(i);
                    listViewSuchErgebnis.Items.Add(party.PartyName);
                }
            }
            else
            {
                var message = new MessageDialog("Leider keine Partys in deiner Nähe.");
                await message.ShowAsync();
            }

            progressRingInDerNaehe.IsEnabled = false;
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;

            this.IsEnabled = true;

        }

        private void Einstellungen_wechselZuMenu(object sender, RoutedEventArgs e)
        {

        }
    }
}
