using App2Night.Logik;
using App2Night.ModelsEnums.Enums;
using App2Night.ModelsEnums.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterErstellen : Page
    {
        public FensterErstellen()
        {
            this.InitializeComponent();
            progressRingErstellen.Visibility = Visibility.Collapsed;
            // MusicGenres in ComboBox anzeigen
            comboBoxErstellenMUSIKRICHTUNG.ItemsSource = Enum.GetValues(typeof(MusicGenre));
            comboBoxErstellenTYP.ItemsSource = Enum.GetValues(typeof(PartyType));
        }

        private void Abbrechen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterHauptansicht));
        }

        private async void Erstellen_wechselPostUndZuAnzeige(object sender, RoutedEventArgs e)
        {
            progressRingErstellen.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            // Überprüfung und Post
            CreatePartyModel partyZuErstellen = new CreatePartyModel();

            // Validieren der Ortsangabe
            Location zuValidieren = new Location();
            zuValidieren.CityName = textBoxErstellenORT.Text;
            zuValidieren.StreetName = textBoxErstellenSTRASSE.Text;
            zuValidieren.HouseNumber = textBoxErstellenHAUSNR.Text;
            zuValidieren.ZipCode = textBoxErstellenPLZ.Text;

            Token tok = await DatenVerarbeitung.aktuellerTokenFuerPost();

            // TODO: Prüfen, ob das geht bzw Nachricht bei ungültiger Adresse
            string erfolg = await BackEndComPartyLogik.ValidateLocation(zuValidieren, tok);

            //TODO: Auf falsche Eingabe reagieren 
            try
            {
                partyZuErstellen.PartyName = textBoxErstellenNAME.Text;

                partyZuErstellen.CityName = textBoxErstellenORT.Text;
                partyZuErstellen.StreetName = textBoxErstellenSTRASSE.Text;
                partyZuErstellen.HouseNumber = textBoxErstellenHAUSNR.Text;
                partyZuErstellen.ZipCode = textBoxErstellenPLZ.Text;

                DateTime zwischenSpeicherDate = new DateTime(DatePickerErstellenDATUM.Date.Year, DatePickerErstellenDATUM.Date.Month, DatePickerErstellenDATUM.Date.Day,
                                                                                        TimePickerErstellenUHRZEIT.Time.Hours, TimePickerErstellenUHRZEIT.Time.Minutes, TimePickerErstellenUHRZEIT.Time.Seconds);

                partyZuErstellen.PartyDate = zwischenSpeicherDate;
                partyZuErstellen.MusicGenre = (MusicGenre)comboBoxErstellenMUSIKRICHTUNG.SelectedItem;
                partyZuErstellen.PartyType = (PartyType)comboBoxErstellenTYP.SelectedItem;
                partyZuErstellen.Description = textBoxErstellenINFOS.Text;
                string preis = textBoxErstellenPREIS.Text;
                partyZuErstellen.Price = Double.Parse(preis);

                bool status = await BackEndComPartyLogik.CreateParty(partyZuErstellen);

                if (status == true)
                {
                    var message = new MessageDialog("Party erfolgreich erstellt!");
                    await message.ShowAsync();
                    this.Frame.Navigate(typeof(FensterHauptansicht));
                }
                else
                {
                    var message = new MessageDialog("Es ist ein Fehler beim Erstellen aufgetreten. Bitte versuche es später erneut.");
                    await message.ShowAsync();
                }

                if (partyZuErstellen.PartyDate < DateTime.Today)
                {
                    Exception FehlerhaftesDatum = new Exception();
                    throw FehlerhaftesDatum;
                }
            }
            catch (Exception ex)
            {
                var message = new MessageDialog("Fehler! Ein oder mehrere Eingaben sind ungültig!\nBeispielsweise wird eine Party in der Vergangenheit angelegt oder die Adresse existiert nicht!");
                await message.ShowAsync();
                return;
            }

            this.IsEnabled = true;
            progressRingErstellen.Visibility = Visibility.Collapsed;

        }
    }
}
