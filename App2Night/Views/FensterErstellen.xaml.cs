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
using Newtonsoft.Json;

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
            Party partyZuErstellen = new Party();

            // Validieren der Ortsangabe
            Location zuValidieren = new Location();
            zuValidieren.CityName = textBoxErstellenORT.Text;
            zuValidieren.StreetName = textBoxErstellenSTRASSE.Text;
            zuValidieren.HouseNumber = textBoxErstellenHAUSNR.Text;
            zuValidieren.ZipCode = textBoxErstellenPLZ.Text;

            Token tok = await DatenVerarbeitung.DatenAusDateiLesenToken();
            bool erfolg = await DatenVerarbeitung.aktuellerToken();

            // Falls noch kein Token angelegt wurde, wird ein neuer erzeugt
            if (tok == null)
            {
                Login nutzer = await DatenVerarbeitung.DatenAusDateiLesenLogin();
                tok = await BackEndComUserLogik.GetToken(nutzer);
                await DatenVerarbeitung.DatenInDateiSchreibenToken(tok);

                if (tok.AccessToken == null)
                {
                    var message = new MessageDialog("Die Email wurde noch nicht bestätigt!");
                    await message.ShowAsync();
                    this.IsEnabled = true;
                    return;
                }
            }

            // Gibt die korrekte Adresse zurück, falls Google sie finden kann
            string adresseLautGoogle = await BackEndComPartyLogik.ValidateLocation(zuValidieren, tok);

            if (adresseLautGoogle != "")
            {
                zuValidieren = JsonConvert.DeserializeObject<Location>(adresseLautGoogle); 
            }

            //TODO: Auf falsche Eingabe reagieren 
            try
            {
                partyZuErstellen.PartyName = textBoxErstellenNAME.Text;

                partyZuErstellen.Location = zuValidieren;

                DateTime zwischenSpeicherDate = new DateTime(DatePickerErstellenDATUM.Date.Year, DatePickerErstellenDATUM.Date.Month, DatePickerErstellenDATUM.Date.Day,
                                                                                        TimePickerErstellenUHRZEIT.Time.Hours, TimePickerErstellenUHRZEIT.Time.Minutes, TimePickerErstellenUHRZEIT.Time.Seconds);

                partyZuErstellen.PartyDate = zwischenSpeicherDate;
                partyZuErstellen.MusicGenre = (MusicGenre)comboBoxErstellenMUSIKRICHTUNG.SelectedItem;
                partyZuErstellen.PartyType = (PartyType)comboBoxErstellenTYP.SelectedItem;

                if (textBoxErstellenINFOS.Text != null)
                {
                    partyZuErstellen.Description = textBoxErstellenINFOS.Text; 
                }
                else
                {
                    partyZuErstellen.Description = "";
                }

                if (textBoxErstellenPREIS.Text != null)
                {
                    string preis = textBoxErstellenPREIS.Text;
                    partyZuErstellen.Price = Double.Parse(preis); 
                }
                else
                {
                    partyZuErstellen.Price = 0;
                }
                

                if (partyZuErstellen.PartyDate < DateTime.Today)
                {
                    Exception FehlerhaftesDatum = new Exception();
                    throw FehlerhaftesDatum;
                }

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
                    this.IsEnabled = true;
                    progressRingErstellen.Visibility = Visibility.Collapsed;
                }

            }
            catch (Exception)
            {
                var message = new MessageDialog("Fehler! Ein oder mehrere Eingaben sind ungültig!\nBeispielsweise wird eine Party in der Vergangenheit angelegt oder die Adresse existiert nicht!");
                await message.ShowAsync();
                this.IsEnabled = true;
                progressRingErstellen.Visibility = Visibility.Collapsed;
                return;
            }

            //this.IsEnabled = true;
            //progressRingErstellen.Visibility = Visibility.Collapsed;

        }
    }
}
