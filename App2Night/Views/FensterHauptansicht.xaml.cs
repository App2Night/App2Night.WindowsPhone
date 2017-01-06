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
using App2Night.Ressources;
using Newtonsoft.Json;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;
using Windows.Foundation;

namespace App2Night.Views
{
    /// <summary>
    /// Die Hauptansicht ist der Mittelpunkt der App. Von hier aus kann der Nutzer: eine neue Party erstellen, Partys in der Nähe abrufen, 
    /// eine Party anzeigen lassen, seine Vormerkungen einsehen und seine Einstellungen bearbeiten
    /// </summary>
    public sealed partial class FensterHauptansicht : Page
    {
        public Party party;
        public static IEnumerable<Party> partyListe;
        static int anzahlPartys = 0;
        int anzahlVorgemerkt = 0;
        int anzahlTeilgenommen = 0;

        public FensterHauptansicht()
        {
            this.InitializeComponent();
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Bei Wechsel auf diese Seite wird eine Hinweismeldung ausgegeben (falls man vom Anmelden oder Registrieren kommt).
        /// Zusätzlich werden die eventuell schon zwischengespeicherten Partys wieder ausgelesen.
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.IsEnabled = false;
            progressRingInDerNaehe.Visibility = Visibility.Visible;

            // Hinweis erscheint nur, wenn man vom Anmelden/Registrieren auf diese Haupansicht kommt
            PageStackEntry vorherigeSeite = Frame.BackStack.Last();
            Type vorherigeSeiteTyp = vorherigeSeite?.SourcePageType;

            if (vorherigeSeiteTyp == (typeof(FensterAnmelden)) || vorherigeSeiteTyp == (typeof(FensterReg)))
            {
                var message = new MessageDialog(Meldungen.Hauptansicht.Nutzungsbedingungen, "Hinweis");
                await message.ShowAsync();
            }

            try
            {
                // Anzeigen der zwischengespeicherten Partys (falls vorhanden)
                partyListe = await DatenVerarbeitung.PartysAuslesen();
            }
            catch (Exception)
            {

            }

            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();

            if (einst.GPSErlaubt == true)
            {
                // Anzeigen der Partys in der "normalen" Liste und ggf. in der Liste für die vorgemerkten Partys.
                if (partyListe != null && partyListe.Any() == true)
                {
                    anzahlPartys = partyListe.Count();

                    for (int durchlauf = 0; durchlauf < anzahlPartys; durchlauf++)
                    {
                        // Liste aller Partys in der Nähe werden in der "normalen" ListView angezeigt
                        party = partyListe.ElementAt(durchlauf);
                        listViewSuchErgebnis.Items.Add(party.PartyName);

                        // Auf der Karte anzeigen
                        PartyAufMapAnzeigen(party);

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

                // Aktuelle Position ermitteln und dies als Kartenmittelpunkt setzen
                var geoLocation = new GeolocationLogik();
                Location location = await geoLocation.GetLocation();
                BasicGeoposition basis = new BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude };
                Geopoint point = new Geopoint(basis);
                mapControlHauptansicht.Center = point;
                mapControlHauptansicht.ZoomLevel = 15;
                mapControlHauptansicht.LandmarksVisible = true; 
            }

            progressRingInDerNaehe.Visibility = Visibility.Collapsed;
            this.IsEnabled = true;
        }

        /// <summary>
        /// Zeigt die übergebene Party auf der Karte an.
        /// </summary>
        /// <param name="party"></param>
        private void PartyAufMapAnzeigen(Party party)
        {
            // Festlegen der Position
            BasicGeoposition partyPosition = new BasicGeoposition() { Latitude = party.Location.Latitude, Longitude = party.Location.Longitude };
            Geopoint partyZentrum = new Geopoint(partyPosition);

            // Icon für Standort Party
            MapIcon partyIcon = new MapIcon();

            partyIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Square44x44Logo.scale-100.png", UriKind.Absolute));
            partyIcon.Location = partyZentrum;
            partyIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            partyIcon.Title = party.PartyName;
            partyIcon.ZIndex = 0;

            mapControlHauptansicht.MapElements.Add(partyIcon);
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
                var message = new MessageDialog(Meldungen.Hauptansicht.AnzeigePartyFehler, "Fehler");
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

            // Listen leeren
            listViewSuchErgebnis.Items.Clear();
            listViewVorgemerkt.Items.Clear();
            listViewTeilnahme.Items.Clear();

            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();

            if (einst.GPSErlaubt == true)
            {
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

                        // Auf der Karte anzeigen
                        PartyAufMapAnzeigen(party);

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
                    var message = new MessageDialog(Meldungen.Hauptansicht.KeinePartysInDerNaehe, "Schade!");
                    await message.ShowAsync();
                } 
            }
            else
            {
                var message = new MessageDialog(Meldungen.Hauptansicht.FehlerGPSNoetig, "Achtung!");
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

            try
            {
                // Aktuelle Position ermitteln
                var geoLocation = new GeolocationLogik();
                pos = await geoLocation.GetLocation();
            }
            catch (Exception)
            {
                var message = new MessageDialog(Meldungen.Hauptansicht.FehlerGPSNoetig, "Achtung!");
                await message.ShowAsync();
                return null;
            }

            // Radius aus UserEinstellungen
            UserEinstellungen einst = await DatenVerarbeitung.UserEinstellungenAuslesen();
            float radius = einst.Radius;

            // Liste der Partys vom BackEnd erhalten
            partyListe = await BackEndComPartyLogik.GetParties(pos, radius);

            // Zwischenspeichern der aktuell angezeigten Partys
            bool erfolg = await ListViewAuslesenUndZwischenspeichern(partyListe);

            if (erfolg == false)
            {
                var message = new MessageDialog(Meldungen.Hauptansicht.FehlerZwischenSpeichern, "Fehler beim Speichern für Offline-Nutzung!");
            }

            return partyListe;
        }

        /// <summary>
        /// Wechselt bei Anklicken einer Party zur Anzeige dieser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTeilnahme_SelectParty(object sender, SelectionChangedEventArgs e)
        {
            AuswahlPartyUndAnzeige(sender);
        }

        /// <summary>
        /// Wechselt bei Anklicken einer Party zur Anzeige dieser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewVorgemerkt_SelectParty(object sender, SelectionChangedEventArgs e)
        {
            AuswahlPartyUndAnzeige(sender);
        }

        /// <summary>
        /// Wechselt bei Anklicken einer Party zur Anzeige dieser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewSuche_SelectParty(object sender, SelectionChangedEventArgs e)
        {
            AuswahlPartyUndAnzeige(sender);
        }

        /// <summary>
        /// Speichert die Partys auf der ListView für die Offline-Nutzung.
        /// </summary>
        /// <param name="liste"></param>
        /// <returns></returns>
        private static async Task<bool> ListViewAuslesenUndZwischenspeichern(IEnumerable<Party> liste)
        {
            bool erfolg = false;

            erfolg = await DatenVerarbeitung.PartysSpeichern(liste);
            
            return erfolg;
        }

    }
}

