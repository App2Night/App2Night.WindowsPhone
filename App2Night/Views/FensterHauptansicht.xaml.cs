using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App2Night.ModelsEnums.Model;
using System.Threading.Tasks;
using Windows.UI.Popups;
using App2Night.Logik;
using App2Night.ModelsEnums.Enums;
using Windows.UI.Xaml.Navigation;

namespace App2Night.Views
{
    /// <summary>
    /// Die Hauptansicht ist der Mittelpunkt der App. Von hier aus kann der Nutzer: eine neue Party erstellen, Partys in der Nähe abrufen, 
    /// eine Party anzeigen lassen, seine Vormerkungen einsehen und seine Einstellungen bearbeiten
    /// </summary>
    public sealed partial class FensterHauptansicht : Page
    {
        public Party party;
        public IEnumerable<Party> partyListe;
        string anzeige = "Um Partys anzuzeigen benötigt die App GPS und eine Internetverbindung. Zum Suchen oder Aktualisieren klicke bitte auf die Lupe unten. Viel Spaß!";
        int anzahlPartys = 0;
        int anzahlVorgemerkt = 0;
        int anzahlTeilgenommen = 0;

        public FensterHauptansicht()
        {
            this.InitializeComponent();
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Hinweis erscheitn nur, wenn man vom Anmelden/Registrieren auf diese Haupansicht kommt
            if (e.SourcePageType == (typeof(FensterAnmelden)) || e.SourcePageType == (typeof(FensterReg)))
            {
                var message = new MessageDialog(anzeige, "Hinweis");
                await message.ShowAsync(); 
            }
        }

        /// <summary>
        /// Wechselt bei Anklicken einer Party zur Anzeige dieser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewSuchErgebnis_AnklickenParty(object sender, SelectionChangedEventArgs e)
        {
            AuswahlPartyUndAnzeige(sender);
        }

        /// <summary>
        /// Wechselt bei Anklicken einer Party zur Anzeige dieser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewVorgemerkt_AnklickenParty(object sender, SelectionChangedEventArgs e)
        {
            AuswahlPartyUndAnzeige(sender);
        }

        /// <summary>
        /// Wählt die angeklickte Party aus, wechselt zum FensterAnzeigen und übergibt dabei die Daten der gewählten Party.
        /// </summary>
        /// <param name="sender"></param>
        private void AuswahlPartyUndAnzeige(object sender)
        {
            // Name des gewählten ListItems auslesen 
            string partyName = ((ListView)sender).SelectedItem.ToString();
            bool partygefunden = false;
            int suchDurchLauf = 0;

            // Solange die Namen der Partys in der Liste mit dem Namen der gewählten Party vergleichen, bis die richtige Party gefunden ist.
            // Begrent wird die Suche durch die Anzahl der Partys in der Liste
            while (partygefunden == false && suchDurchLauf <= anzahlPartys)
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

            // Wenn die Party gefunden wurde, dann Wechsel zu FensterAnzeigen und übergabe der gewählten Party.
            if (partygefunden == true)
            {
                party = partyListe.ElementAt(suchDurchLauf);

                this.Frame.Navigate(typeof(FensterVeranstaltungAnzeigen), party);
            }
            else
            {
                var message = new MessageDialog("Leider ist ein Fehler aufgetreten. Bitte versuche es erneut", "Fehler");
            }
        }

        /// <summary>
        /// Einfacher Wechsel zu FensterErstellen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hinzufuegen_wechselZuErstellen(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterErstellen));
        }

        /// <summary>
        /// Zeigt die Partys in der Umgebung an. Suchradius wird in UserEinstellungen geändert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Suchen_abrufenPartys(object sender, RoutedEventArgs e)
        {
            // Sperren der Oberfläche
            this.IsEnabled = false;
            progressRingInDerNaehe.Visibility = Visibility.Visible;

            // Liste der Partys aus der Nähe 
            partyListe = await btnInDerNaehePartysAbrufen();

            // Anzeigen der Partys in der "normalen" Liste und ggf. in der Liste für die vorgemerkten Partys.
            if (partyListe.Any())
            {
                anzahlPartys = partyListe.Count();

                for (int durchlauf = 0; durchlauf < anzahlPartys; durchlauf++)
                {
                    // Liste aller Partys in der Nähe werden in der "normalen" ListView angezeigt
                    party = partyListe.ElementAt(durchlauf);
                    listViewSuchErgebnis.Items.Add(party.PartyName);

                    // Liste der vorgemerkten Partys werden in einer separaten ListView angezeigt
                    if (party.UserCommitmentState == EventCommitmentState.Noted)
                    {
                        listViewVorgemerkt.Items.Add(party.PartyName);

                        anzahlVorgemerkt++;
                    }

                    // Liste der Partys, bei denen der Nutzer teilnimmt, werden in einer separaten ListView angezeigt
                    if (party.UserCommitmentState == EventCommitmentState.Accepted)
                    {
                        listViewTeilnahme.Items.Add(party.PartyName);

                        anzahlTeilgenommen++;
                    }
                }
            }
            else
            {
                var message = new MessageDialog("Leider keine Partys in deiner Nähe.");
                await message.ShowAsync();
            }

            // Benachrichtigung, wenn keine Party vorgemerkt.
            if (anzahlVorgemerkt == 0)
            {
                // Box mit neuer Nachricht anzeigen
                //textBlockVorgemerktLeereListe.Text = "Du hast noch keine Party vorgemerkt. Vormerken erfolgt über den Stern unten rechts in der Anzeige einer Party.";

            }

            // Benachrichtigung, wenn an keiner Party teilgenommen.
            if (anzahlTeilgenommen == 0)
            {
                // Box mit neuer Nachricht anzeigen
                //textBlockVorgemerktLeereListe.Text = "Du hast bei keiner Party zugesagt. Teilnehmen erfolgt über die Musiknoten unten mittig in der Anzeige einer Party.";
            }

            progressRingInDerNaehe.IsEnabled = false;
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;

            this.IsEnabled = true;
        }

        /// <summary>
        ///  Einfacher Wechsel zu FensterUserEinstellungen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Einstellungen_wechselZuMenu(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterUserEinstellungen));
        }

        /// <summary>
        /// Gibt die aktuellen Partys aus der Umgebung zurück.
        /// </summary>
        /// <returns></returns>
        private static async Task<IEnumerable<Party>> btnInDerNaehePartysAbrufen()
        {
            IEnumerable<Party> partyListe = null;
            Location pos;

            // Aktuelle Position ermitteln
            var geoLocation = new GeolocationLogik();
            pos = await geoLocation.GetLocation();

            // Radius aus UserEinstellungen
            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();
            float radius = einst.Radius;

            // Liste der Partys vom BackEnd erhalten
            partyListe = await BackEndComPartyLogik.GetParties(pos, radius);

            return partyListe;

        }

        //private async void test(object sender, RoutedEventArgs e)
        //{
        //    bool listeLeer = false;

        //    try
        //    {
        //        var items = listViewTeilnahme.Items;
        //    }
        //    catch (Exception)
        //    {
        //        listeLeer = true;
        //    }

        //    if (listeLeer == true)
        //    {
        //        var message = new MessageDialog("Du hast bei keiner Party zugesagt. Um bei einer Party teilzunehmen, klicke bei der Anzeige der Party einfach auf die Musiknoten.", "Hinweis");
        //        await message.ShowAsync();
        //    }
        //}
    }
}

