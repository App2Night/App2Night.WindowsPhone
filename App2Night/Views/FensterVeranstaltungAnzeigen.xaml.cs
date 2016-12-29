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
using App2Night.ModelsEnums.Enums;
using App2Night.Logik;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterVeranstaltungAnzeigen : Page
    {
        public Party uebergebeneParty = new Party();

        public FensterVeranstaltungAnzeigen()
        {
            this.InitializeComponent();
            ProgRingAnzeigen.Visibility = Visibility.Collapsed;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uebergebeneParty = e.Parameter as Party;

            DateTime partyDatumUhrzeit = uebergebeneParty.PartyDate;
            DateTime partyDatum = partyDatumUhrzeit.Date;
            TimeSpan partyUhrzeit = partyDatumUhrzeit.TimeOfDay;
            
            // null möglich!
            textBlVeranstAnzeigenNAME.Text = uebergebeneParty.PartyName;
            textBoxAnzeigenDATUM.Text = partyDatum.ToString("dd/MM/yyyy");
            textBoxAnzeigenUHRZEIT.Text = partyDatumUhrzeit.ToString("HH:mm");
            textBoxAnzeigenORT.Text = uebergebeneParty.Location.CityName;
            textBoxAnzeigenMUSIKRICHTUNG.Text = uebergebeneParty.MusicGenre.ToString();
            textBoxAnzeigenWeitereINFOS.Text = uebergebeneParty.Description;
        }

        private void Zurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private async void Vormerken_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            ProgRingAnzeigen.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            CommitmentParty commitment = new CommitmentParty();
            commitment.Teilnahme = EventCommitmentState.Noted;

            string antwort = await BackEndComPartyLogik.PutPartyCommitmentState(uebergebeneParty, commitment);

            this.IsEnabled = true;
            ProgRingAnzeigen.Visibility = Visibility.Collapsed;

            if (antwort == "200")
            {
                var message = new MessageDialog("Diese Party wurde für dich vorgemerkt!", "Erfolg!");
                await message.ShowAsync();
            }
            else
            {
                var message = new MessageDialog("Es ist ein Fehler aufgetreten. Bitte versuche es später erneut!", "Fehler!");
                await message.ShowAsync();
            }

            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private void AufKarteAnzeigen_wechselZuKartenAnzeige(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterKartenansicht), uebergebeneParty);
        }

        private void Bearbeiten_wechselZuErstellen(object sender, RoutedEventArgs e)
        {

        }

        private async void Teilnehmen_CommitmentStateSetzen(object sender, RoutedEventArgs e)
        {
            CommitmentParty teilnehmen = new CommitmentParty();
            bool zusagen = false;


            if (uebergebeneParty.UserCommitmentState == EventCommitmentState.Rejected  
                || uebergebeneParty.UserCommitmentState == EventCommitmentState.Noted) 
            {
                appBarButtonTeilnehmen.Icon = new SymbolIcon(Symbol.Audio); ;
                appBarButtonTeilnehmen.Label = "Teilnehmen";
                
                teilnehmen.Teilnahme = .EventCommitmentState.Accepted;

                zusagen = true;
            }
            else
            {
                appBarButtonTeilnehmen.Icon = new SymbolIcon(Symbol.Undo); ;
                appBarButtonTeilnehmen.Label = "Absagen";

                teilnehmen.Teilnahme = EventCommitmentState.Rejected;

                zusagen = false;
            }

            ProgRingAnzeigen.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            string teilnahme = await BackEndComPartyLogik.PutPartyCommitmentState(uebergebeneParty, teilnehmen);

            if (teilnahme == "200")
            {
                if (zusagen == true)
                {
                    var message = new MessageDialog("Deine Teilnahme wurde berücksichtigt!", "Viel Spaß!");
                    await message.ShowAsync();
                }
                else
                {
                   var message = new MessageDialog("Deine Absage wurde berücksichtigt!", "Pech gehabt!");
                    await message.ShowAsync();
                }
            }
            else
            {
                var message = new MessageDialog("Schade, wenn Du noch teilnehmen möchtest, versuche es erneut!", "Nicht erfolgreich");
                await message.ShowAsync();

            }
            this.IsEnabled = true;
            ProgRingAnzeigen.Visibility = Visibility.Collapsed;

        }
    }
}
