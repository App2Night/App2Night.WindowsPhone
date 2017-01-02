﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using App2Night.ModelsEnums.Model;
using App2Night.ModelsEnums.Enums;
using App2Night.Logik;
using Windows.UI.Popups;

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
            ProgRingAnzeigen.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Anzeigen der Daten der Party.
        /// Anpassen der Ansicht abhängig vom Nutzer, Stand der Vormerkung und Teilnahme.
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

            // Falls der aktuelle Nutzer der Ersteller der Party ist, wird ihm der Button um zur Bearbeitung zu wechseln angezeigt.
            if (uebergebeneParty.HostedByUser == true)
            {
                appBarButtonBearbeiten.Visibility = Visibility.Visible;
                textBlVeranstAnzeigenNAME.Width = 245;
            }
            else
            {
                appBarButtonBearbeiten.Visibility = Visibility.Collapsed;
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
            ProgRingAnzeigen.Visibility = Visibility.Visible;
            this.IsEnabled = false;

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
            this.IsEnabled = true;
            ProgRingAnzeigen.Visibility = Visibility.Collapsed;


            if (antwort == true)
            {
                // Abhängig davon, ob der Nutzer die Party vormerken will oder nicht, wird ihm eine entsprechende Nachricht ausgegeben.
                if (notiert == true)
                {
                    var message = new MessageDialog("Diese Party wurde für dich vorgemerkt!", "Erfolg!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Noted; 
                }
                else
                {
                    var message = new MessageDialog("Diese Party ist nicht mehr vorgemerkt!", "Erfolg!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Rejected;
                }
            }
            else
            {
                var message = new MessageDialog("Es ist ein Fehler aufgetreten. Bitte versuche es später erneut!", "Fehler!");
                await message.ShowAsync();
            }

            // Zum Schluss wechselt man zur Hauptansicht
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Einfacher Wechsel zur Kartenanzeige. Die anzuzeigende Party wird mitgegeben. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AufKarteAnzeigen_wechselZuKartenAnzeige(object sender, RoutedEventArgs e)
        {
            // TODO: Ersetzen. Karte mit auf Ansicht
            this.Frame.Navigate(typeof(FensterKartenansicht), uebergebeneParty);
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
            ProgRingAnzeigen.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            // Teilnahme/Absage ans BackEnd schicken
            bool teilnahme = await BackEndComPartyLogik.PutPartyCommitmentState(uebergebeneParty, teilnehmen);

            if (teilnahme == true)
            {
                // Abhängig davon, ob der Nutzer teilnehmen oder absagen will, wird ihm eine entsprechende Nachricht ausgegeben.
                if (zusagen == true)
                {
                    var message = new MessageDialog("Deine Teilnahme wurde berücksichtigt!", "Viel Spaß!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Accepted;
                    this.Frame.Navigate(typeof(FensterHauptansicht));
                }
                else
                {
                   var message = new MessageDialog("Deine Absage wurde berücksichtigt!", "Schade!");
                    await message.ShowAsync();
                    uebergebeneParty.UserCommitmentState = EventCommitmentState.Rejected;
                }
            }
            else
            {
                var message = new MessageDialog("Es ist ein Fehler aufgetreten. Bitte versuche es später erneut.", "Fehler!");
                await message.ShowAsync();

            }

            // Entsperren der Oberfläche
            this.IsEnabled = true;
            ProgRingAnzeigen.Visibility = Visibility.Collapsed;

            // Wechsel zur Hauptansicht
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }
    }
}
