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

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungErstellen02 : Page
    {
        Party partyZuErstellen;
        Token tok = new Token();

        public FensterVeranstaltungErstellen02()
        {
            this.InitializeComponent();

            // MusicGenres in ComboBox anzeigen
            comboBoxErstellenMUSIKRICHTUNG.ItemsSource = Enum.GetValues(typeof(MusicGenre));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            partyZuErstellen = e.Parameter as Party;
        }

        private void btnZurueck_wechselZuErstellen01(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterVeranstaltungErstellen));
        }

        private async void btnErstellen_wechselZuAnzeige(object sender, RoutedEventArgs e)
        {
            partyZuErstellen.MusicGenre = (MusicGenre)comboBoxErstellenMUSIKRICHTUNG.SelectedItem;
            partyZuErstellen.Price = Convert.ToInt32(textBoxErstellenPREIS.Text);
            // TODO: Maximale Zeichenzahl beachten
            partyZuErstellen.Description = textBoxErstellenWEITEREINFOS.Text;

            Login temp = new Login();
            temp.Email = "testY@test.de";
            temp.Password = "hallo1234";
            temp.Username = "YvetteLa";

            string id = await BackEndCommunication.BackEndComUser.CreateUser(temp);
            tok = await BackEndCommunication.BackEndComUser.GetToken(temp);

            //tok = await BackEndCommunication.BackEndComUser.RefreshToken(tok);

            string status = await BackEndCommunication.BackEndComParty.CreateParty(partyZuErstellen, tok); 

            if (status == "")
            {
                var message = new MessageDialog("Party nicht erstellt");
                message.ShowAsync();
                this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen), partyZuErstellen);
            }     
        }

        

    }
}
