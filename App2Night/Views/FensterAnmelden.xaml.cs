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
using App2Night.ModelsEnums.Model;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterAnmelden : Page
    {
        public Login anmelden = new Login();

        public FensterAnmelden()
        {
            this.InitializeComponent();
        }

        private void btnZurueck_wechselZuAnmOderReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        private void btnAnmelden_AnmeldenWechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            anmelden.Username = txtBlAnmNutzername.Text;
            anmelden.Email = txtBlAnmEMAIL.Text;
            anmelden.Password = pwBoxPASSWORT.Password;
            // TODO: Nutzereingaben ueberpruefen
            //wenn es stimmt 
            this.Frame.Navigate(typeof(FensterHauptansicht), anmelden);
        }

        private void btnPwVergessen_wechselZuNeuesPW(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterNeuesPW));
        }
    }
}
