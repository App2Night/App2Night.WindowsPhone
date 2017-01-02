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
        int anzahlPartys = 0;

        public FensterHauptansicht()
        {
            this.InitializeComponent();
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;
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
                    }
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
    }
}

