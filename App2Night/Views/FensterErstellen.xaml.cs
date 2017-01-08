using App2Night.Logik;
using App2Night.ModelsEnums.Enums;
using App2Night.ModelsEnums.Model;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using App2Night.Ressources;
using System.Linq;

namespace App2Night.Views
{
    /// <summary>
    /// Auf dieser Seite kann der Nutzer eine Party erstellen. Die Seite dient auch zum Bearbeiten bereits erstellter Partys.
    /// </summary>
    public sealed partial class FensterErstellen : Page
    {
        public Party uebergebeneParty = new Party();
        public bool ueberarbeiten = false;

        public FensterErstellen()
        {
            this.InitializeComponent();
            progressRingErstellen.Visibility = Visibility.Collapsed;
            progressRingErstellen.IsActive = false;

            // MusicGenres und PartyTypen in ComboBox anzeigen
            comboBoxErstellenMUSIKRICHTUNG.ItemsSource = Enum.GetValues(typeof(MusicGenre));
            comboBoxErstellenTYP.ItemsSource = Enum.GetValues(typeof(PartyType));
        }

        /// <summary>
        ///  Abhängig von der Quellseite, von der aus man auf diese Seite gelangt, wird eine Party erstellt oder bearbeitet.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Nimmt die beim Seitenwechsel übergebene Party an (falls vorhanden)
            uebergebeneParty = e.Parameter as Party;

            // Quellseite auslesen
            PageStackEntry vorherigeSeite = Frame.BackStack.Last();
            Type vorherigeSeiteTyp = vorherigeSeite?.SourcePageType;

            if (vorherigeSeiteTyp == (typeof(FensterVeranstaltungAnzeigen)))
            {
                // Falls man von der Seite Anzeigen kommt, wird die Party hier zum Bearbeiten freigegeben und die Buttons dementsprechend angepasst.
                ueberarbeiten = true;
                AppBarButtonErstellen.Icon = new SymbolIcon(Symbol.Edit);
                AppBarButtonErstellen.Label = "Änderungen speichern";

                // Daten der uebergebenen Party anzeigen
                TimeSpan uhrzeit = new TimeSpan(uebergebeneParty.PartyDate.Hour, uebergebeneParty.PartyDate.Minute, uebergebeneParty.PartyDate.Second);

                textBoxErstellenNAME.Text = uebergebeneParty.PartyName;
                DatePickerErstellenDATUM.Date = uebergebeneParty.PartyDate;
                TimePickerErstellenUHRZEIT.Time = uhrzeit;
                textBoxErstellenORT.Text = uebergebeneParty.Location.CityName;
                textBoxErstellenSTRASSE.Text = uebergebeneParty.Location.StreetName;
                textBoxErstellenHAUSNR.Text = uebergebeneParty.Location.HouseNumber;
                textBoxErstellenPLZ.Text = uebergebeneParty.Location.ZipCode;
                textBoxErstellenPREIS.Text = uebergebeneParty.Price.ToString();
                comboBoxErstellenMUSIKRICHTUNG.SelectedItem = uebergebeneParty.MusicGenre;
                comboBoxErstellenTYP.SelectedItem = uebergebeneParty.PartyType;
                textBoxErstellenINFOS.Text = uebergebeneParty.Description;

            }
        }

        /// <summary>
        /// Einfacher Wechsel zur Hauptansicht, falls der Nutzer die Erstellung abbricht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Abbrechen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        /// <summary>
        /// Hier werden die Daten, die der Nutzer eingegeben hat, ausgelesen und abhängig davon, ob diese Party neu erstellt oder bearbeitet wird,
        /// die passende Backend-Methode aufgerufen.
        /// Bei Fehleingaben wird der Nutzer darauf hingewiesen und die Erstellung/Bearbeitung kann fortgesetzt werden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Erstellen_wechselPostUndZuAnzeige(object sender, RoutedEventArgs e)
        {
            // Sperren der Oberfläche
            SperrenDerAnsicht();

            Party partyZuErstellen = new Party();
            bool status = false;

            // Objekt zum Validieren der Ortsangabe
            Location zuValidieren = new Location();
            zuValidieren.CityName = textBoxErstellenORT.Text;
            zuValidieren.StreetName = textBoxErstellenSTRASSE.Text;
            zuValidieren.HouseNumber = textBoxErstellenHAUSNR.Text;
            zuValidieren.ZipCode = textBoxErstellenPLZ.Text;

            // Gibt die korrekte Adresse zurück, falls Google sie finden kann
            string adresseLautGoogle = await BackEndComPartyLogik.ValidateLocation(zuValidieren);

            if (adresseLautGoogle != "")
            {
                zuValidieren = JsonConvert.DeserializeObject<Location>(adresseLautGoogle); 
            }

            // Speichern der Eingaben des Nutzers. Falscheingaben werden abgefangen und es wird eine Fehlermeldung ausgegeben.
            try
            {
                // Beim Bearbeiten wird die Id der vorhandenen Party benötigt
                try
                {
                    if (uebergebeneParty.PartyId != null)
                    {
                        partyZuErstellen.PartyId = uebergebeneParty.PartyId;
                    }
                }
                catch (Exception)
                {
                    // Zur Kenntnis genommen
                }

                partyZuErstellen.PartyName = textBoxErstellenNAME.Text;

                partyZuErstellen.Location = zuValidieren;

                DateTime zwischenSpeicherDate = new DateTime(DatePickerErstellenDATUM.Date.Year, DatePickerErstellenDATUM.Date.Month, DatePickerErstellenDATUM.Date.Day,
                                                                                        TimePickerErstellenUHRZEIT.Time.Hours, TimePickerErstellenUHRZEIT.Time.Minutes, TimePickerErstellenUHRZEIT.Time.Seconds);

                partyZuErstellen.PartyDate = zwischenSpeicherDate;
                partyZuErstellen.MusicGenre = (MusicGenre)comboBoxErstellenMUSIKRICHTUNG.SelectedItem;
                partyZuErstellen.PartyType = (PartyType)comboBoxErstellenTYP.SelectedItem;

                // Die Beschreibung und der Preis sind optional.
                // Deshalb werden Standardwerte benötigt, falls die Felder vom Nutzer leergelassen wurden.
                if (textBoxErstellenINFOS.Text != "")
                {
                    partyZuErstellen.Description = textBoxErstellenINFOS.Text; 
                }
                else
                {
                    // Standardwert
                    partyZuErstellen.Description = "Keine Beschreibung vorhanden.";
                }

                if (textBoxErstellenPREIS.Text != "")
                {
                    string preis = textBoxErstellenPREIS.Text;
                    partyZuErstellen.Price = Int32.Parse(preis); 
                }
                else
                {
                    // Standardwert
                    partyZuErstellen.Price = 0;
                }
                
                // Die zu erstellende/bearbeitende Party darf nicht in der Vergangenheit sein.
                if (partyZuErstellen.PartyDate.Date < DateTime.Today)
                {
                    Exception FehlerhaftesDatum = new Exception();
                    throw FehlerhaftesDatum;
                }

                // Party muss mindestens 2h in der Zukunft stattfinden
                if (partyZuErstellen.PartyDate <= DateTime.Now.AddHours(2))
                {
                    Exception FehlerhaftesDatum = new Exception();
                    throw FehlerhaftesDatum;
                }

                // Hier wird unterschieden, ob die Party bearbeitet oder neu erstellt wird.
                if (ueberarbeiten == false)
                {
                    // Party neu erstellen
                    status = await BackEndComPartyLogik.CreateParty(partyZuErstellen); 
                }
                else
                {
                    // Party bearbeiten
                    status = await BackEndComPartyLogik.UpdatePartyByID(partyZuErstellen);
                }

                if (status == true)
                {
                    var message = new MessageDialog(Meldungen.Erstellen.Erfolg, "Erfolg!");
                    await message.ShowAsync();
                    this.Frame.Navigate(typeof(FensterHauptansicht));
                }
                else
                {
                    if (ueberarbeiten == false)
                    {
                        var message = new MessageDialog(Meldungen.Erstellen.FehlerSpeicher, "Fehler!");
                        await message.ShowAsync();

                        // Entsperren der Oberfläche
                        EntsperrenDerAnsicht();
                    }
                    else
                    {
                        var message = new MessageDialog(Meldungen.Erstellen.FehlerAktualisieren, "Fehler!");
                        await message.ShowAsync();

                        // Entsperren der Oberfläche
                        EntsperrenDerAnsicht();
                    }
                }

            }
            catch (Exception)
            {
                var message = new MessageDialog(Meldungen.Erstellen.UngueltigeEingabe, "Fehler!");
                await message.ShowAsync();

                // Entsperren der Oberfläche
                EntsperrenDerAnsicht();

                return;
            }

            // Entsperren der Oberfläche
            EntsperrenDerAnsicht();

        }

        /// <summary>
        /// Sperrt die Oberfläche.
        /// </summary>
        private void SperrenDerAnsicht()
        {
            progressRingErstellen.Visibility = Visibility.Visible;
            progressRingErstellen.IsActive = true;
            this.AppBarButtonAbbrechen.IsEnabled = false;
            this.AppBarButtonErstellen.IsEnabled = false;
            this.IsEnabled = false;
        }

        /// <summary>
        /// Entsperrt die Oberfläche.
        /// </summary>
        private void EntsperrenDerAnsicht()
        {
            this.IsEnabled = true;
            this.AppBarButtonErstellen.IsEnabled = true;
            this.AppBarButtonAbbrechen.IsEnabled = true;
            progressRingErstellen.Visibility = Visibility.Collapsed;
            progressRingErstellen.IsActive = false;
        }
    }
}
