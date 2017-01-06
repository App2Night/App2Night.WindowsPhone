using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using App2Night.ModelsEnums.Model;
using App2Night.ModelsEnums.Enums;
using App2Night.Logik;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI;
using App2Night.Ressources;
using Windows.Foundation;
using System.Threading.Tasks;

namespace App2Night.Views
{
    /// <summary>
    /// Diese Seite zeigt alle Daten zur einer gewählten Party an. 
    /// Hier kann man Teilnehmen/Voten und der Ersteller der Party kann diese auch bearbeiten. 
    /// </summary>
    public sealed partial class FensterVeranstaltungAnzeigen : Page
    {
        public Party uebergebeneParty = new Party();

        public FensterVeranstaltungAnzeigen()
        {
            this.InitializeComponent();
            progRingAnzeigen.Visibility = Visibility.Collapsed;
            progRingAnzeigen.IsActive = false;
        }

        /// <summary>
        /// Anzeigen der Daten der Party.
        /// Anpassen der Ansicht abhängig vom Nutzer, Stand der Vormerkung und Teilnahme und der Position der Party.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uebergebeneParty = e.Parameter as Party;

            // Auslesen und Anzeigen aller Daten der übergebenen Party
            DateTime partyDatumUhrzeit = uebergebeneParty.PartyDate;
            DateTime partyDatum = partyDatumUhrzeit.Date;
            TimeSpan partyUhrzeit = partyDatumUhrzeit.TimeOfDay;
            
            textBlVeranstAnzeigenNAME.Text = uebergebeneParty.PartyName;
            textBoxAnzeigenDATUM.Text = partyDatum.ToString("dd/MM/yyyy");
            textBoxAnzeigenUHRZEIT.Text = partyDatumUhrzeit.ToString("HH:mm");
            textBoxAnzeigenORT.Text = uebergebeneParty.Location.CityName;
            textBoxAnzeigenMUSIKRICHTUNG.Text = uebergebeneParty.MusicGenre.ToString();
            textBoxAnzeigenWeitereINFOS.Text = uebergebeneParty.Description;
            textBoxAnzahlVOTES.Text = uebergebeneParty.GeneralRating.ToString();

            // Farbliche Hervorhebung der Votes
            if (uebergebeneParty.GeneralRating > 0)
            {
                textBoxAnzahlVOTES.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (uebergebeneParty.GeneralRating < 0)
            {
                textBoxAnzahlVOTES.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                textBoxAnzahlVOTES.Foreground = new SolidColorBrush(Colors.Black);
            }

            // Falls der aktuelle Nutzer der Ersteller der Party ist, wird ihm der Button um zur Bearbeitung zu wechseln angezeigt.
            if (uebergebeneParty.HostedByUser == true)
            {
                appBarButtonBearbeiten.Visibility = Visibility.Visible;
                appBarButtonLoeschen.Visibility = Visibility.Visible;
                textBlVeranstAnzeigenNAME.Width = 245;
            }
            else
            {
                appBarButtonBearbeiten.Visibility = Visibility.Collapsed;
                appBarButtonLoeschen.Visibility = Visibility.Collapsed;
                textBlVeranstAnzeigenNAME.Width = 303;
            }

            // Einstellen der Anzeige abhängig vom Status der Vormerkung
            if (uebergebeneParty.UserCommitmentState != EventCommitmentState.Noted)
            {
                AppBarButtonVormerken.Icon = new SymbolIcon(Symbol.OutlineStar);
                AppBarButtonVormerken.Label = "Vormerken";
            }
            else
            {
                AppBarButtonVormerken.Icon = new SymbolIcon(Symbol.Favorite);
                AppBarButtonVormerken.Label = "Nicht vormerken";
            }

            // Anzeigen der Teilnahme/Absage abhängig von Zu-/Absage
            if (uebergebeneParty.UserCommitmentState != EventCommitmentState.Accepted)
            {
                // Teilnehmen kann man, wenn der aktuelle Status zur Party Rejected oder Noted ist
                appBarButtonTeilnehmen.Icon = new SymbolIcon(Symbol.Audio); ;
                appBarButtonTeilnehmen.Label = "Teilnehmen";

            }
            else
            {
                appBarButtonTeilnehmen.Icon = new SymbolIcon(Symbol.Undo); ;
                appBarButtonTeilnehmen.Label = "Absagen";
            }

            // Voting aktivieren, wenn Party am gleichen Tag und Nutzer teilnimmt
            if (uebergebeneParty.PartyDate.Date == DateTime.Today && uebergebeneParty.UserCommitmentState == EventCommitmentState.Accepted)
            {
                appBarButtonLiken.IsEnabled = true;
                appBarButtonNichtLiken.IsEnabled = true;
            }
            else
            {
                appBarButtonLiken.IsEnabled = false;
                appBarButtonNichtLiken.IsEnabled = false;
            }

            // Info zum Voting anzeigen/nicht anzeigen
            textBlockInfoVoting.Text = Meldungen.Anzeige.InfoVoting;

            // Party auf Karte anzeigen
            MapPartyAnzeigen(uebergebeneParty);
        }

        /// <summary>
        /// Zeigt die Party mittig auf der Karte an.
        /// </summary>
        /// <param name="uebergebeneParty"></param>
        private void MapPartyAnzeigen(Party uebergebeneParty)
        {
            // Festlegen der Position
            BasicGeoposition partyPosition = new BasicGeoposition() { Latitude = uebergebeneParty.Location.Latitude, Longitude = uebergebeneParty.Location.Longitude };
            Geopoint partyZentrum = new Geopoint(partyPosition);

            // Festlegen des Mittelpunkts
            mapControlKarte.Center = partyZentrum;
            mapControlKarte.ZoomLevel = 15;
            mapControlKarte.LandmarksVisible = true;

            // Icon für Standort Party
            MapIcon partyIcon = new MapIcon();

            partyIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Square44x44Logo.scale-100.png", UriKind.Absolute));
            partyIcon.Location = partyZentrum;
            partyIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            partyIcon.Title = uebergebeneParty.PartyName;
            partyIcon.ZIndex = 0;

            mapControlKarte.MapElements.Add(partyIcon);
        }

        /// <summary>
        /// Einfacher Wechsel zur Hauptansicht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zurueck_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Hier wird die Party für den Nutzer vorgemerkt/nicht mehr vorgemerkt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Vormerken_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            CommitmentParty commitment = new CommitmentParty();
            bool notiert = false;

            // Sperren der Oberfläche
            this.IsEnabled = false;
            progRingAnzeigen.Visibility = Visibility.Visible;
            progRingAnzeigen.IsActive = true;

            // Hier wird der Status der Vormerkung notiert
            if (uebergebeneParty.UserCommitmentState != EventCommitmentState.Noted)
            {
                commitment.Teilnahme = EventCommitmentState.Noted;

                notiert = true;
            }
            else
            {
                commitment.Teilnahme = EventCommitmentState.Rejected;

                notiert = false;
            }

            // Vormerkung ans BackEnd schicken
            bool antwort = await BackEndComPartyLogik.PutPartyCommitmentState(uebergebeneParty, commitment);

            // Entsperrung der Oberfläche
            progRingAnzeigen.Visibility = Visibility.Collapsed;
            progRingAnzeigen.IsActive = false;
            this.IsEnabled = true;


            if (antwort == true)
            {
                // Abhängig davon, ob der Nutzer die Party vormerken will oder nicht, wird ihm eine entsprechende Nachricht ausgegeben.
                if (notiert == true)
                {
                    var message = new MessageDialog(Meldungen.Anzeige.ErfolgVormerken, "Erfolg!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Noted; 
                }
                else
                {
                    var message = new MessageDialog(Meldungen.Anzeige.ErfolgVergessen, "Erfolg!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Rejected;
                }
            }
            else
            {
                var message = new MessageDialog(Meldungen.Anzeige.FehlerVormerkenVergessen, "Fehler!");
                await message.ShowAsync();
            }

            // Zum Schluss wechselt man zur Hauptansicht
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Einfacher Wechsel zum Bearbeiten der Party. Die zu bearbeitende Party wird mitgegeben. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bearbeiten_wechselZuErstellen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterErstellen), uebergebeneParty);
        }

        /// <summary>
        /// Hier wird die Teilnahme/Absage für die Party vorgenommen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Teilnehmen_CommitmentStateSetzen(object sender, RoutedEventArgs e)
        {
            CommitmentParty teilnehmen = new CommitmentParty();
            bool zusagen = false;

            // Hier wird der Status der Teilnahme notiert
            if (uebergebeneParty.UserCommitmentState != EventCommitmentState.Accepted) 
            {          
                teilnehmen.Teilnahme = EventCommitmentState.Accepted;

                zusagen = true;
            }
            else
            {
                teilnehmen.Teilnahme = EventCommitmentState.Rejected;

                zusagen = false;
            }

            // Sperren der Oberfläche
            this.IsEnabled = false;
            progRingAnzeigen.Visibility = Visibility.Visible;
            progRingAnzeigen.IsActive = true;

            // Teilnahme/Absage ans BackEnd schicken
            bool teilnahme = await BackEndComPartyLogik.PutPartyCommitmentState(uebergebeneParty, teilnehmen);

            if (teilnahme == true)
            {
                // Abhängig davon, ob der Nutzer teilnehmen oder absagen will, wird ihm eine entsprechende Nachricht ausgegeben.
                if (zusagen == true)
                {
                    var message = new MessageDialog(Meldungen.Anzeige.ErfolgTeilnahme, "Viel Spaß!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Accepted;
                    this.Frame.Navigate(typeof(FensterHauptansicht));
                }
                else
                {
                    var message = new MessageDialog(Meldungen.Anzeige.ErfolgAbsage, "Schade!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Rejected;
                }
            }
            else
            {
                var message = new MessageDialog(Meldungen.Anzeige.FehlerTeilnahmeAbsage, "Fehler!");
                await message.ShowAsync();

            }

            // Entsperren der Oberfläche
            progRingAnzeigen.Visibility = Visibility.Collapsed;
            progRingAnzeigen.IsActive = false;
            this.IsEnabled = true;

            // Wechsel zur Hauptansicht
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Gibt dem Nutzer die Möglichkeit seine erstelle Party zu löschen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Loeschen_PartyLoeschen(object sender, RoutedEventArgs e)
        {
            // Teile übernommen von http://stackoverflow.com/questions/35392306/does-the-messagedialog-class-for-uwp-apps-support-three-buttons-on-mobile  
            const int OK = 1;
            const int ABBR = 2;

            // Dialog, der abfragt, ob der Nutzer die Party wirklich löschen will
            var message = new MessageDialog(Meldungen.Anzeige.AbsicherungLoeschen, "Achtung!");
            message.Commands.Add(new UICommand { Label = "Ja", Id = OK });
            message.Commands.Add(new UICommand { Label = "Abbrechen", Id = ABBR });
            var reaktion = await message.ShowAsync();

            var id = (int)(reaktion?.Id ?? ABBR);

            // Falls der Nutzer bestätigt, dass er die Party löschen will
            if (id == 1)
            {
                this.IsEnabled = false;
                progRingAnzeigen.Visibility = Visibility.Visible;
                progRingAnzeigen.IsActive = true;

                bool erfolg = await BackEndComPartyLogik.DeletePartyByID(uebergebeneParty);

                progRingAnzeigen.Visibility = Visibility.Collapsed;
                progRingAnzeigen.IsActive = false;
                this.IsEnabled = true;

                if (erfolg == true)
                {
                    message = new MessageDialog(Meldungen.Anzeige.ErfolgLoeschen, "Erfolg!");
                    await message.ShowAsync();
                }
                else
                {
                    message = new MessageDialog(Meldungen.Anzeige.MisserfolgLoeschen, "Fehler!");
                    await message.ShowAsync();
                }
            }
            else
            {
                message = new MessageDialog(Meldungen.Anzeige.AbbrechenLoeschen, "Abbrechen");
                await message.ShowAsync();
            }
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Reagiert auf den Upvote vom Nutzer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Liken_sendeUpvote(object sender, RoutedEventArgs e)
        {
            bool like = true;

            bool erfolg = await SendeVote(like);

            if (erfolg == true)
            {
                var message = new MessageDialog(Meldungen.Anzeige.ErfolgUpvoting, "Erfolg!");
                await message.ShowAsync();
            }
            else
            {
                var message = new MessageDialog(Meldungen.Anzeige.FehlerVoting, "Fehler!");
                await message.ShowAsync();
            }
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }
        
        /// <summary>
        /// Reagiert auf den Downvote vom Nutzer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Dislike_sendeDownvote(object sender, RoutedEventArgs e)
        {
            bool like = false;

            bool erfolg = await SendeVote(like);

            if (erfolg == true)
            {
                var message = new MessageDialog(Meldungen.Anzeige.ErfolgDownvoting, "Erfolg!");
                await message.ShowAsync();
            }
            else
            {
                var message = new MessageDialog(Meldungen.Anzeige.FehlerVoting, "Fehler!");
                await message.ShowAsync();
            }

            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        ///  Nimmt die Bewertung entgegen und schickt sie ans BackEnd.
        /// </summary>
        /// <param name="like">Vote vom Nutzer</param>
        /// <returns></returns>
        private async Task<bool> SendeVote(bool like)
        {
            bool erfolg = false;
            PartyVoting voting = new PartyVoting();

            // Da wir diese Bewertung nicht abfragen, setzen wir diese Werte auf 0 (= nicht bewertet)
            voting.LocationRating = 0;
            voting.MoodRating = 0;
            voting.PriceRating = 0;

            if (like == true)
            {
                voting.GeneralRating = 1;

                erfolg = await BackEndComPartyLogik.PutPartyRating(uebergebeneParty, voting);
            }
            else
            {
                voting.GeneralRating = -1;

                erfolg = await BackEndComPartyLogik.PutPartyRating(uebergebeneParty, voting);
            }

            return erfolg;
        }
    }
}
