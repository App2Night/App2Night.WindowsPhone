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
using App2Night.ModelsEnums.Enums;
using Windows.UI.Popups;
using App2Night.Logik;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungErstellen02 : Page
    {
        CreatePartyModel partyZuErstellen;
        Token tok = new Token();

        public FensterVeranstaltungErstellen02()
        {
            this.InitializeComponent();
            progRingErstellen02.Visibility = Visibility.Collapsed;
            // MusicGenres in ComboBox anzeigen
            comboBoxErstellenMUSIKRICHTUNG.ItemsSource = Enum.GetValues(typeof(MusicGenre));
            comboBoxErstellenTYP.ItemsSource = Enum.GetValues(typeof(PartyType));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            partyZuErstellen = e.Parameter as CreatePartyModel;
        }

        private void btnZurueck_wechselZuErstellen01(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen));
        }

        private void btnErstellen_wechselZuAnzeige(object sender, RoutedEventArgs e)
        {
            //this.IsEnabled = false;
            //progRingErstellen02.Visibility = Visibility.Visible;
            //partyZuErstellen.MusicGenre = (MusicGenre)comboBoxErstellenMUSIKRICHTUNG.SelectedItem;
            //partyZuErstellen.PartyType = (PartyType)comboBoxErstellenTYP.SelectedItem;
            //partyZuErstellen.Description = textBoxErstellenWEITEREINFOS.Text;
            //string preis = textBoxErstellenPREIS.Text;
            //partyZuErstellen.Price = Double.Parse(preis);

            ////bool status = await BackEndComPartyLogik.CreateParty(partyZuErstellen);

            //progRingErstellen02.Visibility = Visibility.Collapsed;

            //if (status == true)
            //{
            //    var message = new MessageDialog("Party erfolgreich erstellt!");
            //    await message.ShowAsync();
            //    this.Frame.Navigate(typeof(FensterHauptansicht));
            //}
            //else
            //{
            //    var message = new MessageDialog("Es ist ein Fehler beim Erstellen aufgetreten. Bitte versuche es später erneut.");
            //    await message.ShowAsync();
            //}
            //this.IsEnabled = true;
        }     

    }
}
