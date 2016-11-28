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
using Windows.Data.Json;
using App2Night.ModelsEnums;
using App2Night.ModelsEnums.Model;
using System.Threading.Tasks;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterHauptansichtKopieren : Page
    {
        public IEnumerable<Party> partyListe;
        public Party party; 

        public FensterHauptansichtKopieren()
        {
            this.InitializeComponent();
            progressRingInDerNaehe.Visibility = Visibility.Collapsed;
            ListView listViewSuchErgebnis = new ListView();
            
        }

        private void btnErstellen_wechselZuVeranstErstellen(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnSuche_wechselZuVeranstSuchen(object sender, RoutedEventArgs e)
        {
           
        }

        private async void btnVeranstInDerNaehe_GetPartys(object sender, RoutedEventArgs e)
        { 
           



        }

        private void listViewSuchErgebnisse_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void listView_ClickOnItem(object sender, SelectionChangedEventArgs e)
        {
            
           
            
        }

    }
}
